# Arena Survivor

Protótipo 2D de um jogo de sobrevivência feito em **Unity**

## Estudantes

- Ana Luiza S. S. Duarte
- Anna Sophia F. Santos
- Pâmela C. Machado

## Como executar

1. Abra a pasta do projeto no **Unity Hub** (versão 2022.3.62f3 ou compatível).
2. Abra a cena `Assets/Scenes/SampleScene.unity`.
3. **Monte a interface (UI) no editor** conforme o passo a passo abaixo (somente na primeira vez — a interface fica salva na cena).
4. Aperte **Play**. A arena, o jogador e os inimigos são criados automaticamente pelo script `GameBootstrap`.

## Como montar a UI no editor (obrigatório)

A interface **não é criada por código** — monte a hierarquia abaixo na cena:

```
Canvas                     (UI > Canvas)
│  Render Mode: Screen Space - Overlay
│  adicionar componentes: CanvasScaler e GraphicRaycaster
│  CanvasScaler: UI Scale Mode = Scale With Screen Size
│                Reference Resolution = 1920 x 1080, Match = 0.5
│
├── MenuPanel              (UI > Panel, cor escura, âncoras = stretch para preencher a tela)
│   ├── TitleText          (UI > Text, "ARENA SURVIVOR", tamanho ~90)
│   ├── PlayButton         (UI > Button, texto "JOGAR")
│   └── QuitButton         (UI > Button, texto "SAIR")
│
├── HudPanel               (UI > Panel transparente, âncoras = stretch)
│   ├── HealthBarBackground  (UI > Image, retângulo pequeno no canto superior esquerdo)
│   │   └── HealthFill       (UI > Image, Type = Filled, Fill Method = Horizontal, cor verde)
│   ├── HealthText          (UI > Text sobre a barra, ex.: "Vida: 100 / 100")
│   ├── WaveText            (UI > Text centralizado no topo, ex.: "Onda: - / 3")
│   ├── ScoreText           (UI > Text no canto superior direito, ex.: "Pontos: 0")
│   └── AnnouncementText    (UI > Text grande centralizado, desativado no início) — opcional
│
├── VictoryPanel           (UI > Panel verde escuro, âncoras = stretch, iniciar desativado)
│   ├── VictoryTitleText    (UI > Text, "VITÓRIA!")
│   ├── VictoryScoreText    (UI > Text, ex.: "Pontuação final: 0")
│   ├── ReplayVictoryButton (UI > Button, "JOGAR NOVAMENTE")
│   └── MenuVictoryButton   (UI > Button, "MENU")
│
└── GameOverPanel          (UI > Panel vermelho escuro, âncoras = stretch, iniciar desativado)
    ├── GameOverTitleText   (UI > Text, "DERROTA")
    ├── GameOverScoreText   (UI > Text)
    ├── ReplayGameOverButton (UI > Button, "JOGAR NOVAMENTE")
    └── MenuGameOverButton  (UI > Button, "MENU")

UIManager                  (GameObject vazio na cena, com o componente UIManager)
```

Depois, selecione o GameObject **UIManager** e arraste cada elemento da hierarquia para o campo correspondente no Inspector (Painéis, HUD, Botões). Os cliques dos botões são conectados automaticamente pelo código.

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
- Interface montada manualmente no editor da Unity (sem imagens geradas por código na UI).

## Pendentes

- Nenhum requisito do enunciado está pendente.

## Créditos dos assets

Sprites da arena/jogador/inimigos e sons são gerados por código durante a execução; nenhum asset de terceiros foi utilizado. Interface (UI) 100% montada no editor. Ferramenta: Unity 2022.3 LTS com URP 2D.
