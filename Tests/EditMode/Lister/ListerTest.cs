using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class ListerTests
{
    private Lister<ScriptableObject> lister;
    private ScriptableObject element1;
    private ScriptableObject element2;

    [SetUp]
    public void SetUp()
    {
        lister = new Lister<ScriptableObject>();
        element1 = ScriptableObject.CreateInstance<ScriptableObject>();
        element2 = ScriptableObject.CreateInstance<ScriptableObject>();
    }

    [Test]
    public void AddListable_WhenElementAdded_ShouldContainElementInList()
    {
        // Arrange
        lister.AddListable(element1);

        // Act
        lister.AddListable(element2);

        // Assert
        Assert.Contains(element2, lister.Siblings, "Element2 should be added to the list.");
    }

    [Test]
    public void AddListable_WhenOnAddActionSet_ShouldInvokeOnAddAction()
    {
        // Arrange
        bool onAddInvoked = false;
        lister.OnAdd = () => onAddInvoked = true;

        // Act
        lister.AddListable(element1);

        // Assert
        Assert.IsTrue(onAddInvoked, "OnAdd action should be invoked when an element is added.");
    }

    [Test]
    public void RemoveListable_WhenElementRemoved_ShouldNotContainElementInList()
    {
        // Arrange
        lister.AddListable(element1);

        // Act
        lister.RemoveListable(element1);

        // Assert
        Assert.IsFalse(lister.Siblings.Contains(element1), "Element1 should be removed from the list.");
    }

    [Test]
    public void RemoveListable_WhenOnRemoveActionSet_ShouldInvokeOnRemoveAction()
    {
        // Arrange
        lister.AddListable(element1);
        bool onRemoveInvoked = false;
        lister.OnRemove = () => onRemoveInvoked = true;

        // Act
        lister.RemoveListable(element1);

        // Assert
        Assert.IsTrue(onRemoveInvoked, "OnRemove action should be invoked when an element is removed.");
    }

    [Test]
    public void Sync_AfterAddingElements_ShouldSynchronizeSiblingListWithElementsList()
    {
        // Arrange
        lister.AddListable(element1);
        lister.AddListable(element2);

        // Act
        lister.Sync();

        // Assert
        Assert.AreEqual(2, lister.Siblings.Count, "Sibling count should be 2 after synchronization.");
        Assert.Contains(element1, lister.Siblings, "Element1 should be present in the sibling list.");
        Assert.Contains(element2, lister.Siblings, "Element2 should be present in the sibling list.");
    }

    [Test]
    public void OrderedElements_WhenElementsAdded_ShouldReturnOrderedListOfElements()
    {
        // Arrange
        lister.AddListable(element2);
        lister.AddListable(element1);

        // Act
        var orderedElements = Lister<ScriptableObject>.OrderedElements();

        // Assert
        Assert.AreEqual(2, orderedElements.Count, "Ordered element count should be 2.");
        Assert.AreEqual(element1, orderedElements[0], "Element1 should be the first element in the ordered list.");
        Assert.AreEqual(element2, orderedElements[1], "Element2 should be the second element in the ordered list.");
    }
}
