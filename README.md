# Arena Survivor

Protótipo 2D de um jogo de sobrevivência feito em **Unity 2022.3 (URP 2D)** para a atividade avaliativa.

## Estudantes

- Ana Luiza S. S. Duarte
- Anna Sophia F. Santos
- Pâmela C. Machado

## Como executar

1. Abra a pasta do projeto no **Unity Hub** (versão 2022.3.62f3 ou compatível).
2. Abra a cena `Assets/Scenes/SampleScene.unity`.
3. Aperte **Play**. Todo o jogo (arena, jogador, inimigos e interface) é criado automaticamente pelo script `GameBootstrap` — nenhuma configuração manual de cena é necessária.

## Controles

| Ação | Tecla / Botão |
|---|---|
| Mover | WASD ou setas |
| Atirar (mira no mouse) | Botão esquerdo do mouse |
| Atirar na direção do movimento | Espaço |
| Botões da interface | Mouse |

## Funcionalidades implementadas

- **Jogador**: movimentação 2D delimitada pelos limites da arena, 100 pontos de vida, ataque à distância com projéteis (direção, velocidade e destruição ao atingir alvos ou paredes).
- **Inimigos**: perseguem o jogador, causam dano por contato (com tempo de recarga), morrem ao zerar a vida e concedem **100 pontos**.
- **Sistema de ondas**: 3 ondas progressivas (5, 8 e 12 inimigos); cada onda só começa após a eliminação total da anterior; **vitória ao derrotar a 3ª onda**.
- **Pontuação** exibida em tempo real durante a partida.
- **4 telas**: Menu Inicial (Jogar/Sair), Gameplay (vida, pontuação e contador de onda), Vitória (pontuação final + jogar novamente) e Game Over (pontuação final + jogar novamente).
- Efeitos sonoros gerados por código (sem assets externos).

## Pendentes

- Nenhum requisito do enunciado está pendente.
- Melhorias opcionais: mais de um tipo de inimigo, power-ups, sprites/artes próprias.

## Estrutura de pastas

```
Assets/
  Scenes/        Cena do projeto
  Settings/      Configurações URP 2D
  Scripts/
    Core/        GameManager, WaveManager, ArenaBuilder, Bootstrap, Sfx, sprites
    Player/      PlayerController, PlayerHealth
    Enemies/     Enemy
    Combat/      Projectile
    UI/          UIManager (todas as telas)
```

## Créditos dos assets

Todos os visuais (sprites de formas) e sons são **gerados por código** durante a execução — nenhum asset de terceiros foi utilizado. Ferramenta: Unity 2022.3 LTS com URP 2D.
