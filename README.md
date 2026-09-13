# MinecraftMapApp — Mapeamento Topográfico de Minecraft Bedrock

Aplicativo desktop (Windows Forms, C#/.NET) para marcar, organizar e visualizar
coordenadas de mundos de Minecraft (Superfície e Nether), com suporte a rotas
e gerenciamento de múltiplos mundos.

## Funcionalidades

- Marcar pontos no mapa com nome, coordenadas (X, Y, Z) e nota opcional.
- Alternar entre as dimensões Superfície e Nether.
- Pesquisar pontos já marcados.
- Calcular a distância entre dois pontos.
- Criar rotas ("João e Maria") ligando vários pontos em sequência.
- Excluir rotas salvas.
- Editar ou excluir pontos já marcados.
- Gerenciar múltiplos mundos (criar, selecionar, renomear, salvar, excluir).
- Salvamento automático em disco a cada alteração.

## Como usar

### Marcar um ponto novo
1. Clique em **"+ Adicionar Ponto"**.
2. Preencha nome, coordenadas X/Y/Z e, se quiser, uma nota.
3. Clique em **"MARCAR PONTO"**. O ponto aparece no mapa na dimensão atual.

### Editar ou excluir um ponto
- Dê duplo clique no ponto no mapa para abrir a janela de edição.

### Calcular distância entre dois pontos
- Clique em um ponto e depois em outro para ver a distância entre eles.

### Criar uma rota (João e Maria)
1. Clique no botão **"JOÃO E MARIA"** para ativar o modo.
2. Clique nos pontos, na ordem que quer ligar.
3. Clique no botão de novo para finalizar e salvar a rota.

### Excluir uma rota salva
- Clique com o **botão direito** no botão "JOÃO E MARIA".
- Escolha a rota que quer excluir no menu que aparece.

### Gerenciar mundos
- Use o gerenciador de mundos para criar um mundo novo, trocar de mundo,
  renomear ou excluir um mundo salvo.

## Atalhos

| Atalho | Ação |
|---|---|
| Ctrl + N | Alterna entre Superfície e Nether |
| Scroll do mouse | Zoom no mapa |
| Clique e arraste | Move o mapa |
| Clique em 2 pontos | Mostra a distância entre eles |
| Duplo clique num ponto | Abre a edição do ponto |
| Clique direito em "JOÃO E MARIA" | Abre o menu de exclusão de rotas |

## Requisitos

- Windows
- .NET (versão usada no projeto)

## Status

Versão atual: **v1.0.0**
