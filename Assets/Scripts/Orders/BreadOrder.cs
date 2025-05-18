using UnityEngine;

public class BreadOrder : Order
{
    [SerializeField] GameObject m_clientSpeechBox;

    public override int[] ingredients { get; set; }
    protected override GameObject clientSpeechBox { get => m_clientSpeechBox; set => m_clientSpeechBox = value; }
    protected override GameObject clientSpeechBoxObj { get; set; }

    private void Awake()
    {
        ingredients = new int[] { 0 };
    }
}
