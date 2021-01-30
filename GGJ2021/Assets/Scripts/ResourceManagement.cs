using EnergyBarToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagement : MonoBehaviour
{
    private class Resource
    {
        public EnergyBar bar;
        public FilledRendererUGUI gui;

        public Resource(EnergyBar bar, FilledRendererUGUI gui)
        {
            this.bar = bar;
            this.gui = gui;
        }
    }
    private Board board;
    private Dictionary<PieceType, Resource> pieceTypeToResource;

    void Start()
    {
        board = FindObjectOfType<Board>();

        pieceTypeToResource = new Dictionary<PieceType, Resource>();

        var bars = FindObjectsOfType<EnergyBar>();
        for (int i = 0; i < bars.Length; i++)
        {
            var bar = bars[i];
            var type = (PieceType)i;
            var resource = new Resource(bar, bar.GetComponent<FilledRendererUGUI>());
            resource.gui.spriteBarColor = PieceMapping.typeToColor[type];

            Debug.Log(resource.gui.spriteBarColor);

            pieceTypeToResource.Add(type, resource);
        }

        board.OnPiecesMatched.AddListener(IncreaseResourcesForMatchedPieces);
    }

    private void IncreaseResourcesForMatchedPieces(HashSet<Piece> matched)
    {
        foreach (Piece p in matched)
        {
            Debug.Log("increasing for " + p.Type);
            pieceTypeToResource[p.Type].bar.valueCurrent++;
        }
    }
}
