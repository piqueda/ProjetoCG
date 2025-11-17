using UnityEngine;
using UnityEngine.AI; 

public class WolfController : EnemyBase
{
    [Header("Componentes do Lobo")]
    private NavMeshAgent agent; // O agente de navegação

    [Header("Configurações de Distância")]
    public float chaseRange = 10f; // Distância para começar a perseguir
    public float attackRange = 2f; // Distância para começar a atacar

    // Sobrescreve o Start da classe base
    protected override void Start()
    {
        base.Start(); // Chama o Start do EnemyBase (encontra o Player, pega o Animator)
        
        agent = GetComponent<NavMeshAgent>();
        
        // Garante que o agente esteja ativo
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent não encontrado no lobo!");
        }
    }

    void Update()
    {
        if (playerTransform == null || agent == null) return;

        // 1. Calcular a distância
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // 2. Lógica de Comportamento
        if (distanceToPlayer <= attackRange)
        {
            // Perto o suficiente: Ataca
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            // Na distância de perseguição: Persegue
            ChasePlayer();
        }
        else
        {
            // Longe: Fica parado/Patrulha (se quiser adicionar Patrulha aqui)
            Idle();
        }

        // *OPCIONAL: Atualizar parâmetro de velocidade de animação*
        // Se você tiver um parâmetro Float 'Speed' no Animator para Walk/Idle
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    private void ChasePlayer()
    {
        // Define o alvo de navegação como a posição do jogador
        agent.isStopped = false; // Garante que o agente pode se mover
        agent.SetDestination(playerTransform.position);

        // Opcional: Rotaciona o lobo para a direção do player antes de atacar
        // transform.LookAt(playerTransform); 
    }

    private void AttackPlayer()
    {
        agent.isStopped = true; // Para o movimento antes de atacar
        
        // Vira para o player para garantir que o ataque seja na direção correta
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        
        // Aciona o Animator Trigger
        animator.SetTrigger("AttackL");
    }

    private void Idle()
    {
        agent.isStopped = true; // Garante que o agente está parado
        // Se você tiver uma animação de Idle, garanta que Speed seja 0.
    }
}