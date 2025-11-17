using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [Header("Configuração da Fruta")]
    public GameObject fruitPrefab;

    [Header("Quantidade de Frutas")]
    public int minFruits = 5;
    public int maxFruits = 11;

    [Header("Área de Spawn")]
    public float spawnRadius = 5f;
    [Tooltip("Altura inicial para o Raycast. Deve ser alta o suficiente para 'pegar' o chão.")]
    public float raycastStartHeight = 10f; 
    
    [HideInInspector] public float fruitColliderRadius;
    // Define quais camadas o Raycast deve atingir (ex: Terrain ou Ground)
    public LayerMask groundLayer; 

    void Start()
    {
        SphereCollider collider = fruitPrefab.GetComponent<SphereCollider>();
    if (collider != null)
    {
        // O raio define a distância do centro até a borda
        fruitColliderRadius = collider.radius * fruitPrefab.transform.localScale.x;
    }
    else
    {
        Debug.LogError("O Prefab da Fruta deve ter um SphereCollider anexado!");
    }

        SpawnFruitsOnGround();
    }

    public void SpawnFruitsOnGround()
    {
        int numberOfFruitsToSpawn = Random.Range(minFruits, maxFruits);

        for (int i = 0; i < numberOfFruitsToSpawn; i++)
        {
            // 1. Gera uma posição aleatória no plano XZ (horizontal)
            Vector3 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);

            // 2. Define o ponto inicial do Raycast (acima da posição de spawn)
            Vector3 rayStart = new Vector3(spawnPosition.x, spawnPosition.y + raycastStartHeight, spawnPosition.z);
            
            RaycastHit hit;
            
            // 3. Lança o Raycast para baixo
            if (Physics.Raycast(rayStart, Vector3.down, out hit, raycastStartHeight + 1f, groundLayer))
            {
                // Se o Raycast atingir algo na camada 'groundLayer', usamos essa altura.
                Vector3 finalPosition = hit.point;

                finalPosition.y += fruitColliderRadius;

                // 4. Instancia a fruta na posição exata do chão
                GameObject newFruit = Instantiate(
                    fruitPrefab, 
                    finalPosition, 
                    Quaternion.identity 
                );

                // 5. ATIVAÇÃO INTELIGENTE: A fruta já está no chão, então ela deve estar FIXA.
                FruitDropController dropController = newFruit.GetComponent<FruitDropController>();
                if (dropController != null)
                {
                    // Chamamos a função Drop, mas agora ela SÓ precisa aplicar o impulso/giro visual
                    // e manter a fruta cinemática (fixa), se configurado no Controller.
                    dropController.InitialPlacement(); 
                }
            }
        }
    }
}