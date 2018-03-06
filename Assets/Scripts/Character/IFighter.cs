using Knights.Enums;

public interface IFighter
{
    TypeOfFighter typeOfFighter { get; set; }
    bool IsItMyTurn { get; set; }
    void Attack(Knights.Characters.Character whomToAttack);
}