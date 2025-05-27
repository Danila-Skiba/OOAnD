using System.Collections;

namespace SpaceBattle.Tests
{
    public class CustomBehaviorDictionaryTests
    {
        private readonly Dictionary<string, object> _baseDict = new()
        {
            ["existing"] = "base_value",
            ["common"] = "base_common"
        };

        private readonly Dictionary<string, Func<object>> _behavior = new()
        {
            ["dynamic"] = () => "behavior_value",
            ["common"] = () => "behavior_common"
        };

        [Fact]
        public void Getter_ReturnsBehaviorValue_WhenKeyExistsInBehavior()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            Assert.Equal("behavior_value", wrapper["dynamic"]);
        }

        [Fact]
        public void Getter_ReturnsBaseValue_WhenKeyNotInBehavior()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            Assert.Equal("base_value", wrapper["existing"]);
        }

        [Fact]
        public void Getter_BehaviorHasPriority_WhenKeyInBoth()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            Assert.Equal("behavior_common", wrapper["common"]);
        }

        [Fact]
        public void Getter_ThrowsKeyNotFoundException_WhenKeyMissing()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            Assert.Throws<KeyNotFoundException>(() => wrapper["missing"]);
        }

        [Fact]
        public void Setter_UpdatesBaseDict_ForNormalKey()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            wrapper["existing"] = "new_value";
            Assert.Equal("new_value", _baseDict["existing"]);
        }

        [Fact]
        public void Setter_Throws_WhenKeyReservedForBehavior()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            Assert.Throws<InvalidOperationException>(() => wrapper["dynamic"] = "new_value");
        }

        [Fact]
        public void Add_Throws_WhenKeyReservedForBehavior()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            Assert.Throws<ArgumentException>(() => wrapper.Add("dynamic", "value"));
        }

        [Fact]
        public void Remove_Throws_WhenKeyReservedForBehavior()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            Assert.Throws<InvalidOperationException>(() => wrapper.Remove("dynamic"));
        }

        [Fact]
        public void ContainsKey_ReturnsTrue_ForBehaviorKey()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            Assert.True(wrapper.ContainsKey("dynamic"));
            Assert.True(wrapper.ContainsKey("existing"));
        }

        [Fact]
        public void TryGetValue_ReturnsTrue_ForBehaviorKey()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            Assert.True(wrapper.TryGetValue("dynamic", out var value));
            Assert.Equal("behavior_value", value);
        }

        [Fact]
        public void Values_ContainsAllValues()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            var values = wrapper.Values;
            Assert.Contains("base_value", values);
            Assert.Contains("behavior_value", values);
            Assert.Contains("behavior_common", values);
        }

        [Fact]
        public void Count_ReturnsCombinedUniqueKeyCount()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            Assert.Equal(3, wrapper.Count); // existing, common, dynamic
        }

        [Fact]
        public void Clear_OnlyAffectsBaseDictionary()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            wrapper.Clear();
            Assert.Empty(_baseDict);
            Assert.True(wrapper.ContainsKey("dynamic")); // Behavior keys remain
        }

        [Fact]
        public void Enumerator_YieldsAllItems()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            var result = new Dictionary<string, object>();
            foreach (var kvp in wrapper)
            {
                result.Add(kvp.Key, kvp.Value);
            }

            Assert.Equal("behavior_value", result["dynamic"]);
            Assert.Equal("base_value", result["existing"]);
            Assert.Equal("behavior_common", result["common"]);
        }

        [Fact]
        public void CopyTo_CopiesAllItems()
        {
            var wrapper = new CustomBehaviorDictionary(_baseDict, _behavior);
            var array = new KeyValuePair<string, object>[3];
            wrapper.CopyTo(array, 0);

            var result = new Dictionary<string, object>();
            foreach (var kvp in array)
            {
                result.Add(kvp.Key, kvp.Value);
            }

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void IsReadOnly_ShouldReflectBaseDictionary()
        {
            var readOnlyDict = new Dictionary<string, object> { ["test"] = "value" }.AsReadOnly();
            var wrapper = new CustomBehaviorDictionary(readOnlyDict, new Dictionary<string, Func<object>>());

            Assert.True(wrapper.IsReadOnly);
        }

        [Fact]
        public void IsReadOnly_ShouldBeFalseForWritableDictionary()
        {
            var wrapper = new CustomBehaviorDictionary(new Dictionary<string, object>(), new Dictionary<string, Func<object>>());

            Assert.False(wrapper.IsReadOnly);
        }

        [Fact]
        public void AddKeyValuePair_ShouldAddToBaseDictionary()
        {
            var dict = new Dictionary<string, object>();
            var wrapper = new CustomBehaviorDictionary(dict, new Dictionary<string, Func<object>>());
            var item = new KeyValuePair<string, object>("newKey", "newValue");

            wrapper.Add(item);

            Assert.Equal("newValue", dict["newKey"]);
        }

        [Fact]
        public void AddKeyValuePair_ShouldThrowForBehaviorKey()
        {
            var behavior = new Dictionary<string, Func<object>> { ["reserved"] = () => "null" };
            var wrapper = new CustomBehaviorDictionary(new Dictionary<string, object>(), behavior);
            var item = new KeyValuePair<string, object>("reserved", "value");

            Assert.Throws<ArgumentException>(() => wrapper.Add(item));
        }

        [Fact]
        public void ContainsKeyValuePair_ShouldReturnTrueForExistingItem()
        {

            var dict = new Dictionary<string, object> { ["key"] = "value" };
            var wrapper = new CustomBehaviorDictionary(dict, new Dictionary<string, Func<object>>());
            var item = new KeyValuePair<string, object>("key", "value");

            var res = wrapper.Contains(item);

            Assert.True(res);
        }

        [Fact]
        public void ContainsKeyValuePair_ShouldReturnFalseForWrongValue()
        {
            var dict = new Dictionary<string, object> { ["key"] = "value" };
            var wrapper = new CustomBehaviorDictionary(dict, new Dictionary<string, Func<object>>());
            var item = new KeyValuePair<string, object>("key", "wrongValue");

            Assert.DoesNotContain(item, wrapper);
        }

        [Fact]
        public void ContainsKeyValuePair_ShouldReturnFalseForMissingKey()
        {

            var wrapper = new CustomBehaviorDictionary(new Dictionary<string, object>(), new Dictionary<string, Func<object>>());
            var item = new KeyValuePair<string, object>("missing", "value");

            Assert.DoesNotContain(item, wrapper);
        }

        [Fact]
        public void ContainsKeyValuePair_ShouldReturnFalse_WhenKeyIsMissingInBothDictionaries()
        {

            var wrapper = new CustomBehaviorDictionary(
                new Dictionary<string, object>(),
                new Dictionary<string, Func<object>>());
            var item = new KeyValuePair<string, object>("nonexistent", "value");

            var result = wrapper.Contains(item);

            Assert.False(result);
        }

        [Fact]
        public void RemoveKeyValuePair_ShouldReturnTrueAndRemove()
        {
            var dict = new Dictionary<string, object> { ["key"] = "value" };
            var wrapper = new CustomBehaviorDictionary(dict, new Dictionary<string, Func<object>>());
            var item = new KeyValuePair<string, object>("key", "value");

            var result = wrapper.Remove(item);

            Assert.True(result);
            Assert.Empty(dict);
        }

        [Fact]
        public void RemoveKeyValuePair_ShouldThrowForBehaviorKey()
        {
            var behavior = new Dictionary<string, Func<object>> { ["reserved"] = () => "null" };
            var wrapper = new CustomBehaviorDictionary(new Dictionary<string, object>(), behavior);
            var item = new KeyValuePair<string, object>("reserved", "value");

            Assert.Throws<InvalidOperationException>(() => wrapper.Remove(item));
        }

        [Fact]
        public void IEnumerableGetEnumerator_ShouldHandleEmptyCollections()
        {
            var wrapper = new CustomBehaviorDictionary(
                new Dictionary<string, object>(),
                new Dictionary<string, Func<object>>());

            var count = Enumerable.Range(0, int.MaxValue)
            .TakeWhile(_ => ((IEnumerable)wrapper).GetEnumerator().MoveNext())
            .Count();

            Assert.Equal(0, count);
        }

        [Fact]
        public void TryGetValue_ShouldReturnBaseValue_WhenKeyOnlyInBaseDictionary()
        {
            var baseDict = new Dictionary<string, object> { ["baseOnly"] = "base_value" };
            var behavior = new Dictionary<string, Func<object>>();
            var wrapper = new CustomBehaviorDictionary(baseDict, behavior);

            var result = wrapper.TryGetValue("baseOnly", out var value);

            Assert.True(result);
            Assert.Equal("base_value", value);
        }
    }
}
