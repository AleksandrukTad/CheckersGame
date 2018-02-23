using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewEditModeTest : MonoBehaviour{

	[Test]
	public void NewEditModeTestSimplePasses() {
        // Use the Assert class to test conditions.
        //Arrange
        var game = gameObject.GetComponent<BoardManager>();
        var expectedPiece = game.GetComponent<Piece>();

        expectedPiece.x = 2;
        expectedPiece.y = 2;
        game.GenerateBoard();
        //Act
        game.SelectPiece(2, 2);
        //Assert
        Assert.Equals(expectedPiece, game.selectedPiece);
    }

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator NewEditModeTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
