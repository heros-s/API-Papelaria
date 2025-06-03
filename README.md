# API - Papelaria Empresarial 📦✏️

Este projeto é uma API RESTful desenvolvida em C# com Entity Framework Core e MySQL para simular a gestão de uma papelaria empresarial.

---

## 🔧 Tecnologias

- ASP.NET Core
- Entity Framework Core
- MySQL (XAMPP)
- RESTful APIs

---

## 📚 Funcionalidades da API

A API está organizada em módulos que cobrem as operações principais de uma papelaria:

### ✅ Materiais
- Cadastro, edição, listagem e exclusão de materiais.

### ✅ Estoque
- Controle de quantidades por material.
- A exclusão de um material com estoque é bloqueada.

### ✅ Funcionários
- Registro, edição, listagem e remoção de funcionários.
- Armazena nome, cargo, salário e data de contratação.

### ✅ Carrinho de Compras
- Criação de carrinho.
- Adição e remoção de itens.
- Visualização e limpeza do carrinho.

### ✅ Vendas
- Concretiza uma venda a partir de um carrinho.
- Atualiza o estoque automaticamente.
- Deleta o carrinho após a venda.

### ✅ Máquina de Estados
- Cada venda possui um status (`EmCarrinho`, `PagamentoPendente`, `Concluída`) controlado por uma enum.

### ✅ Contas a Pagar
- Simulação simples de despesas (salários dos funcionários).

### ✅ Contas a Receber
- Lista as vendas realizadas (simulando entradas de receita).

### ✅ Relatórios de Contabilidade
- Exibe o total de receitas (vendas), despesas (salários) e lucro.
