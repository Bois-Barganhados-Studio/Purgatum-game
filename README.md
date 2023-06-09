# Purgatum & Purgatum-game
Gameplay do DEMO Alpha 1.0: https://www.youtube.com/watch?v=IZDh3Thddsk
# Trabalho Interdisciplinar 4 - PUC Minas
# Arthur Ruiz, Daniel Benevides, Edmar Melandes, Felipe Moura, Gustavo Gomes, Leon Júnio, Pedro Pampolini, Vinicius Gabriel
# COMO JOGAR:
## Teclado:
* 1. Movimentos: WASD (Cima, Baixo, Esquerda e Direita)
* 2. Rolamento: CTRL (Movimento para escapar de ataques)
* 3. Bater e causar dano: ESPAÇO
* 4. Trocar de arma: Q
* 5. Interagir com itens: E (Baús, Escadas, poções e armas)
* 6. Menu de pausa: ESC
## Descrição
RogueLike com mapas procedurais, sistema de acampamentos, IA adaptativa, arte isométrica e muito mais! Desenvolvido na Unity, purgatum é a proposta de um jogo rogue like isometrico com mapas gerados proceduralmente, salas aleatórias, IA que aprende com suas runs, inimigos e mecanicas de RPG, sistema de upgrades e muitos outros sistemas internos.

## Sinopse

- O personagem é um experimento do mago
- lobby é um poço onde tem os mobs desprezados pelo mago
- já começa com o personagem caindo no poço
- personagem fala com npc e tem ações com o interior do poço
- o mago despreza o personagem e sempre que o mesmo morre joga para o fim do poço
- durante o primeiro e segundo loop vai liberar pedaços da história (contextualização)
- motivação é salvar todo mundo da torre e manter sua comunidade viva (o poço dos desolados)
- descobrir npcs durante o caminho da torre
- sobe os níveis até chegar no mago
- No mago pode encontrar o final caso derote o mesmo


## Caracteristicas

- Torre (estrutura de níveis)
- Mago (IA)
- Rogue Like baseado em RPG
- PC (porte de mobile)
- Nome do jogo: Purgatum
- Purgatorio ou poço em que se encontra os mobs

## Mecânicas

- Mundo procedural baseado em blocos (tileMaps)
- Inimigos aleatórios por nível
- Numero de salas por nível aleatório
- Estrutura das dungeons (níveis) procedurais
- Visão isométrica (2D com aspecto 3D)
- Inimigo principal como dispositivo inteligente (IA)
- Escolha dos níveis para personagem baseados nos dados coletados do usuário em outras runs
- Armas no estilo RPG clássico (espadas, machados, arco, lanças etc)
- Inimigos com inteligência scriptada e aparência pré definida
- Spawns de mobs gerados junto com o mundo de forma procedural
- Jogo com velocidade de combate dinâmica
- Combate baseado em dano em área/long range
- Lobby principal do jogo com estrutura poço (pequena sociedade no fundo do poço)
- NPCS para interação em níveis e que depois podem ser encontrados no lobby principal
- Mecânismo de melhoria de habilidades do personagem (atributos padrão: força, velocidade, destreza, sabedoria etc)
## GRAFO
O algoritmo de grafos implementado foi o de corte de arestas e o de caminhamento/Busca em largura na geração de mundos. Cada sala é gerada individualmente e modelada como nodo em um grafo, após todas as salas serem geradas caminhos são criados entre as mesmas. Cada sala tem uma estrutura diferente e uma possibilidade nova em uma grid de 16 salas por andar. Logo o total de mapas possiveis para o jogo são: 16 salas x (20x(10)x20x(10) tamanho de cada sala vezes o tamanho de cada bloco) x 20x4  (total de portas por lado) = +/- 5.120.000 De mapas diferentes!
## Inteligência Artificial
Título: Explorando o Backpropagation e o Algoritmo A* no Desenvolvimento do jogo Purgatum
# Introdução:
Neste trabalho, exploramos dois algoritmos essenciais no desenvolvimento de jogos: o Backpropagation e o Algoritmo A*. O Backpropagation foi aplicado para validar se um player sobreviverá ou não em uma "run" do jogo, enquanto o Algoritmo A* foi utilizado para a movimentação dos inimigos em um plano 2D. Ambos desempenharam papéis fundamentais na criação de um mundo proceduralmente gerado e na melhoria da experiência do jogador.
# Backpropagation:
O Backpropagation é um algoritmo de treinamento utilizado em redes neurais artificiais. No contexto do nosso jogo, implementamos uma rede neural capaz de prever se um player vai sobreviver ou não em uma determinada "run". Para isso, utilizamos um conjunto de dados de treinamento composto por múltiplas "runs" do jogo, onde cada "run" tinha características específicas, como o ambiente do jogo, as habilidades do jogador e a presença de inimigos. O algoritmo de Backpropagation permitiu que a rede neural fosse treinada para aprender a associar as características das "runs" do jogo com a sobrevivência ou morte do jogador. Durante o treinamento, os parâmetros da rede neural foram ajustados iterativamente para minimizar a diferença entre as saídas previstas e as saídas reais. Uma vez treinada, a rede neural foi capaz de analisar as características de uma nova "run" e prever se o jogador sobreviveria ou não. Essa informação foi utilizada para ajustar os parâmetros de criação procedural do mundo do jogo, de forma a tornar as "runs" mais desafiadoras e adequadas ao nível de habilidade do jogador.
# Algoritmo A*:
O Algoritmo A* é uma técnica amplamente utilizada em jogos para determinar a rota mais eficiente entre dois pontos em um mapa. No nosso jogo, implementamos o Algoritmo A* para controlar a movimentação dos inimigos em um plano 2D.
O Algoritmo A* considera o mapa do jogo como uma grade, onde cada célula representa uma posição no plano. Ele avalia as células vizinhas a partir da
posição atual do inimigo, calculando o custo estimado para alcançar o objetivo. Esse custo é calculado com base em uma heurística que leva em consideração a distância entre a célula atual e o objetivo, além de possíveis obstáculos no caminho. Utilizando esse cálculo de custo, o Algoritmo A* constrói uma árvore de busca e seleciona a rota mais eficiente até o objetivo. Essa rota é então seguida pelo inimigo, permitindo uma movimentação inteligente e desafiadora.
# Conclusão:
A combinação do Backpropagation e do Algoritmo A* desempenhou um papel fundamental no desenvolvimento do nosso jogo. O Backpropagation permitiu que uma rede neural aprendesse a prever se um jogador sobreviveria em uma determinada "run", permitindo a criação de desafios personalizados e adequados ao jogador. Por sua vez, o Algoritmo A* possibilitou uma movimentação inteligente dos inimigos, garantindo uma experiência de jogo mais imersiva e desafiadora. A aplicação desses algoritmos demonstra a importância do uso de técnicas de inteligência artificial no desenvolvimento de jogos, proporcionando interações mais complexas e enriquecedoras para os jogadores. A contínua evolução dessas técnicas promete trazer avanços ainda mais significativos para a indústria de jogos no futuro.
