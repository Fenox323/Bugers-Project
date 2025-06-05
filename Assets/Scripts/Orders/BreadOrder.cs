using UnityEngine;

public class BreadOrder : Order
{
    [SerializeField] GameObject m_clientSpeechBox;
    [SerializeField] float m_patience;
    [SerializeField] int m_score;

    public override int[] ingredients { get; set; }
    protected override GameObject clientSpeechBox { get => m_clientSpeechBox; set => m_clientSpeechBox = value; }
    protected override GameObject clientSpeechBoxObj { get; set; }
    public override float patience { get => m_patience; protected set => m_patience = value; }
    public override int score { get => m_score; protected set => m_score = value; }

    private void Awake()
    {
        ingredients = new int[] { 0, 0 };
    }
}
