using System;
using System.Collections.Generic;
using LINQ;

namespace ReinventingTheWheel.Tests.LINQ;

public class LinqExtensionsTests
{
    [Fact]
    public void Where_WhenItemsMatchPredicate_ReturnsOnlyMatchingItems()
    {
        int[] source = [1, 2, 3, 4, 5];

        List<int> result = source
            .Where(x => x % 2 == 0)
            .ToList();

        Assert.Equal([2, 4], result);
    }

    [Fact]
    public void Select_WhenSelectorIsApplied_ReturnsProjectedItems()
    {
        int[] source = [1, 2, 3];

        List<string> result = source
            .Select(x => $"Number: {x}")
            .ToList();

        Assert.Equal(["Number: 1", "Number: 2", "Number: 3"], result);
    }

    [Fact]
    public void Count_WhenSourceHasItems_ReturnsNumberOfItems()
    {
        int[] source = [1, 2, 3, 4];

        int result = source.Count();

        Assert.Equal(4, result);
    }

    [Fact]
    public void Count_WhenPredicateIsProvided_ReturnsNumberOfMatchingItems()
    {
        int[] source = [1, 2, 3, 4, 5, 6];

        int result = source.Count(x => x % 2 == 0);

        Assert.Equal(3, result);
    }

    [Fact]
    public void Any_WhenSourceHasItems_ReturnsTrue()
    {
        int[] source = [1, 2, 3];

        bool result = source.Any();

        Assert.True(result);
    }

    [Fact]
    public void Any_WhenSourceIsEmpty_ReturnsFalse()
    {
        int[] source = [];

        bool result = source.Any();

        Assert.False(result);
    }

    [Fact]
    public void Any_WhenAnyItemMatchesPredicate_ReturnsTrue()
    {
        int[] source = [1, 3, 4];

        bool result = source.Any(x => x % 2 == 0);

        Assert.True(result);
    }

    [Fact]
    public void Any_WhenNoItemMatchesPredicate_ReturnsFalse()
    {
        int[] source = [1, 3, 5];

        bool result = source.Any(x => x % 2 == 0);

        Assert.False(result);
    }

    [Fact]
    public void First_WhenSourceHasItems_ReturnsFirstItem()
    {
        int[] source = [10, 20, 30];

        int result = source.First();

        Assert.Equal(10, result);
    }

    [Fact]
    public void First_WhenSourceIsEmpty_ThrowsInvalidOperationException()
    {
        int[] source = [];

        Assert.Throws<InvalidOperationException>(() => source.First());
    }

    [Fact]
    public void First_WhenPredicateIsProvided_ReturnsFirstMatchingItem()
    {
        int[] source = [1, 3, 4, 6];

        int result = source.First(x => x % 2 == 0);

        Assert.Equal(4, result);
    }

    [Fact]
    public void First_WhenNoItemMatchesPredicate_ThrowsInvalidOperationException()
    {
        int[] source = [1, 3, 5];

        Assert.Throws<InvalidOperationException>(() => source.First(x => x % 2 == 0));
    }

    [Fact]
    public void FirstOrDefault_WhenSourceHasItems_ReturnsFirstItem()
    {
        int[] source = [10, 20, 30];

        int result = source.FirstOrDefault();

        Assert.Equal(10, result);
    }

    [Fact]
    public void FirstOrDefault_WhenSourceIsEmpty_ReturnsDefaultValue()
    {
        int[] source = [];

        int result = source.FirstOrDefault();

        Assert.Equal(0, result);
    }

    [Fact]
    public void FirstOrDefault_WhenPredicateIsProvided_ReturnsFirstMatchingItem()
    {
        int[] source = [1, 3, 4, 6];

        int result = source.FirstOrDefault(x => x % 2 == 0);

        Assert.Equal(4, result);
    }

    [Fact]
    public void FirstOrDefault_WhenNoItemMatchesPredicate_ReturnsDefaultValue()
    {
        int[] source = [1, 3, 5];

        int result = source.FirstOrDefault(x => x % 2 == 0);

        Assert.Equal(0, result);
    }

    [Fact]
    public void All_WhenAllItemsMatchPredicate_ReturnsTrue()
    {
        int[] source = [2, 4, 6];

        bool result = source.All(x => x % 2 == 0);

        Assert.True(result);
    }

    [Fact]
    public void All_WhenAnyItemDoesNotMatchPredicate_ReturnsFalse()
    {
        int[] source = [2, 3, 6];

        bool result = source.All(x => x % 2 == 0);

        Assert.False(result);
    }

    [Fact]
    public void All_WhenSourceIsEmpty_ReturnsTrue()
    {
        int[] source = [];

        bool result = source.All(x => x % 2 == 0);

        Assert.True(result);
    }

    [Fact]
    public void Take_WhenCountIsLessThanSourceLength_ReturnsFirstItems()
    {
        int[] source = [1, 2, 3, 4, 5];

        List<int> result = source
            .Take(3)
            .ToList();

        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public void Take_WhenCountIsGreaterThanSourceLength_ReturnsAllItems()
    {
        int[] source = [1, 2, 3];

        List<int> result = source
            .Take(10)
            .ToList();

        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public void Take_WhenCountIsZero_ReturnsEmptyList()
    {
        int[] source = [1, 2, 3];

        List<int> result = source
            .Take(0)
            .ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void Skip_WhenCountIsLessThanSourceLength_ReturnsRemainingItems()
    {
        int[] source = [1, 2, 3, 4, 5];

        List<int> result = source
            .Skip(2)
            .ToList();

        Assert.Equal([3, 4, 5], result);
    }

    [Fact]
    public void Skip_WhenCountIsGreaterThanSourceLength_ReturnsEmptyList()
    {
        int[] source = [1, 2, 3];

        List<int> result = source
            .Skip(10)
            .ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void Skip_WhenCountIsZero_ReturnsAllItems()
    {
        int[] source = [1, 2, 3];

        List<int> result = source
            .Skip(0)
            .ToList();

        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public void ToList_WhenSourceHasItems_ReturnsListWithSameItems()
    {
        int[] source = [1, 2, 3];

        List<int> result = source.ToList();

        Assert.Equal([1, 2, 3], result);
    }

    [Fact]
    public void ToList_WhenSourceIsAlreadyList_ReturnsNewListInstance()
    {
        List<int> source = [1, 2, 3];

        List<int> result = source.ToList();

        Assert.NotSame(source, result);
        Assert.Equal([1, 2, 3], result);
    }
}