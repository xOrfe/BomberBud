using System;
using System.Collections;
using Project.Scripts.Managers;
using UnityEngine;
using Utils = Project.Scripts.Utilities.Utilities;

namespace Project.Scripts.Characters
{
    public class Bomb : Content
    {
        private void Start()
        {
            StartCoroutine(Explode(3f));
        }

        private IEnumerator Explode(float time)
        {
            yield return new WaitForSeconds(time);
            Vector2Int matrixScale = LevelManager.Instance.LevelDefinitionScriptable.MapDefinition.MatrixScale;
            for (int i = -2; i < +3; i++)
            {
                int chunk = Utils.GetIndexFromCoord(CurrentChunk,matrixScale) + i;
                DestroyChunk(chunk,matrixScale);
                if (i == 0) continue;
                chunk = Utils.GetIndexFromCoord(CurrentChunk,matrixScale) + (i * matrixScale.x);
                DestroyChunk(chunk,matrixScale);
            }

            this.Destroy();
        }

        private void DestroyChunk(int chunk,Vector2Int matrixScale)
        {
            if (chunk >= 0  && chunk < LevelManager.Instance.LevelDefinitionScriptable.MapDefinition.MatrixLength)
            {
                foreach (var content in LevelManager.Instance.MapChunkMatrix[chunk].ContentsRigid)
                    if (content.isDestroyable) content.Destroy();
                foreach (var content in LevelManager.Instance.MapChunkMatrix[chunk].ContentsNonRigid)
                    if (content.isDestroyable) content.Destroy();
            }
        }
    }
}