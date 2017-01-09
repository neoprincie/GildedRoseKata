using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GildedRoseKata.Core;
using System.Linq;

namespace GildedRoseKata.Tests
{
    [TestClass]
    public class GildedRoseTests
    {
        private GildedRose _app;
        private List<Item> _items;

        [TestInitialize]
        public void SetUp()
        {
            var items = new List<Item>()
            {
                new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 },
                new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 },
                new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 },
                new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 },
                new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 15,
                    Quality = 20
                },
                new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 },
                new Item { Name = "Spoiled Item", Quality = 10, SellIn = 0 },
                new Item { Name = "Low Quality Item", Quality = 1, SellIn = 1 },
                new Item { Name = "Conjured Mana Cake", Quality = 5, SellIn = 5 },
                new Item { Name = "Conjured Water", Quality = 7, SellIn = 7 }
            };

            _app = new GildedRose(items);
            _items = _app.GetItems();
        }

        [TestMethod]
        public void AllItems_ShouldHaveSellInValue()
        {
            var agedBrie = _items.FirstOrDefault(i => i.Name == "Aged Brie");

            Assert.AreEqual(2, agedBrie.SellIn);
        }

        [TestMethod]
        public void AllItems_ShouldHaveQualityValue()
        {
            var dexterityVest = _items.FirstOrDefault(i => i.Name == "+5 Dexterity Vest");

            Assert.AreEqual(20, dexterityVest.Quality);
        }

        [TestMethod]
        public void AtEndOfEachDay_SellInAndQualityAreLowered()
        {
            var dexterityVest = _items.FirstOrDefault(i => i.Name == "+5 Dexterity Vest");

            UpdateQuality();

            Assert.AreEqual(9, dexterityVest.SellIn);
            Assert.AreEqual(19, dexterityVest.Quality);
        }

        [TestMethod]
        public void OnceSellByDateHasPassed_QualityDegradesTwiceAsFast()
        {
            var spoiledItem = _items.FirstOrDefault(i => i.Name == "Spoiled Item");

            UpdateQuality();

            Assert.AreEqual(-1, spoiledItem.SellIn);
            Assert.AreEqual(8, spoiledItem.Quality);
        }

        [TestMethod]
        public void QualityIsNeverNegative()
        {
            var lowQualityItem = _items.FirstOrDefault(i => i.Name == "Low Quality Item");

            UpdateQuality();
            UpdateQuality();

            Assert.AreEqual(-1, lowQualityItem.SellIn);
            Assert.AreEqual(0, lowQualityItem.Quality);
        }

        [TestMethod]
        public void AgedBrieActuallyIncreasesInQuality()
        {
            var agedBrie = _items.FirstOrDefault(i => i.Name == "Aged Brie");

            UpdateQuality();

            Assert.AreEqual(1, agedBrie.SellIn);
            Assert.AreEqual(1, agedBrie.Quality);
        }

        [TestMethod]
        public void QualityIsNeverAboveFifty()
        {
            var agedBrie = _items.FirstOrDefault(i => i.Name == "Aged Brie");

            for (var i = 0; i < 60; i++)
                UpdateQuality();
            
            Assert.AreEqual(50, agedBrie.Quality);
        }

        [TestMethod]
        public void Sulfuras_BeingLegendary_NeverHasToBeSoldOrReducesQuality()
        {
            var sulfuras = _items.FirstOrDefault(i => i.Name == "Sulfuras, Hand of Ragnaros");

            UpdateQuality();

            Assert.AreEqual(0, sulfuras.SellIn);
            Assert.AreEqual(80, sulfuras.Quality);
        }

        [TestMethod]
        public void BackstagePasses_IncreaseInQuality_RegularlyAboveTenDays()
        {
            var backstagePass = _items.FirstOrDefault(i => i.Name == "Backstage passes to a TAFKAL80ETC concert");

            UpdateQuality();

            Assert.AreEqual(14, backstagePass.SellIn);
            Assert.AreEqual(21, backstagePass.Quality);
        }

        [TestMethod]
        public void BackstagePasses_IncreaseInQuality_ByTwoTenDaysOrLess()
        {
            var backstagePass = _items.FirstOrDefault(i => i.Name == "Backstage passes to a TAFKAL80ETC concert");

            for(var i = 0; i < 6; i ++)
                UpdateQuality();

            Assert.AreEqual(9, backstagePass.SellIn);
            Assert.AreEqual(27, backstagePass.Quality);
        }

        [TestMethod]
        public void BackstagePasses_IncreaseInQuality_ByThreeFiveDaysOrLess()
        {
            var backstagePass = _items.FirstOrDefault(i => i.Name == "Backstage passes to a TAFKAL80ETC concert");

            for (var i = 0; i < 11; i++)
                UpdateQuality();

            Assert.AreEqual(4, backstagePass.SellIn);
            Assert.AreEqual(38, backstagePass.Quality);
        }

        [TestMethod]
        public void BackstagePasses_DropsToZeroQuality_AfterConcert()
        {
            var backstagePass = _items.FirstOrDefault(i => i.Name == "Backstage passes to a TAFKAL80ETC concert");

            for (var i = 0; i < 16; i++)
                UpdateQuality();

            Assert.AreEqual(-1, backstagePass.SellIn);
            Assert.AreEqual(0, backstagePass.Quality);
        }

        [TestMethod]
        public void ConjuredItems_DegradeTwiceAsFast()
        {
            var conjuredManaCake = _items.FirstOrDefault(i => i.Name == "Conjured Mana Cake");
            var conjuredWater = _items.FirstOrDefault(i => i.Name == "Conjured Water");

            UpdateQuality();

            Assert.AreEqual(3, conjuredManaCake.Quality);
            Assert.AreEqual(5, conjuredWater.Quality);
        }

        private void UpdateQuality()
        {
            _app.UpdateQuality();
            _items = _app.GetItems();
        }
    }
}
