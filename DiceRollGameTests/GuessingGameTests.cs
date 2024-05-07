using Game;
using Moq;
using NUnit.Framework;
using UserCommunication;

namespace DiceRollGameTests
{
    public class Tests
    {
        private Mock<IDice> _DiceMock;
        private Mock<IUserCommunication> _IUserCommunicationMock;
        private GuessingGame _GuessingGame;
            
        [SetUp]
        public void Setup()
        {
            _DiceMock=new Mock<IDice>();
            _IUserCommunicationMock= new Mock<IUserCommunication>();
            _GuessingGame = new GuessingGame(_DiceMock.Object, _IUserCommunicationMock.Object);

        }

        [Test]
        public void ThePlayer_GuessCorrect_OnFIrstTry()
        {
            int NumOnDice = 3;

            _DiceMock.Setup(Mock=>Mock.Roll()).Returns(NumOnDice);
            _IUserCommunicationMock.Setup(Mock=>Mock.ReadInteger(It.IsAny<string>())).Returns(NumOnDice);

            var game = _GuessingGame.Play();

            Assert.AreEqual(GameResult.Victory, game);
        }

        [Test]
        public void ThePlayer_GuessCorrect_OnThirdTry()
        {
            int NumOnDice = 3;
            _DiceMock.Setup(Mock => Mock.Roll()).Returns(NumOnDice);
            _IUserCommunicationMock.SetupSequence(Mock=>Mock.ReadInteger(It.IsAny<string>())).Returns(1).Returns(2).Returns(NumOnDice);
            var game = _GuessingGame.Play();
            Assert.AreEqual(GameResult.Victory, game);
        }

        [Test]
        public void ThePlayer_NeverGuess()
        {
            int NumOnDice = 3;
            _DiceMock.Setup(Mock => Mock.Roll()).Returns(NumOnDice);
            _IUserCommunicationMock.SetupSequence(Mock => Mock.ReadInteger(It.IsAny<string>())).Returns(1).Returns(2).Returns(4);
            var game = _GuessingGame.Play();
            Assert.AreEqual(GameResult.Loss, game);
        }

        [TestCase(GameResult.Victory, "You win!")]
        [TestCase(GameResult.Loss, "You lose")]
        public void PrintResult(
        GameResult gameResult, string expectedMessage)
        {

            _GuessingGame.PrintResult(gameResult);

            _IUserCommunicationMock.Verify(
                mock => mock.ShowMessage(expectedMessage));
        }

    }
}