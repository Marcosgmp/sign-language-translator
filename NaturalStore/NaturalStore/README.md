# NaturalStore — Sistema de Produtos Naturais

Projeto acadêmico do professor Silas Santiago **NaturalStore** Simula em console uma loja de produtos naturais.

---

## 📁 Estrutura da Solução

```
NaturalStore/
├── Domain/           
│   ├── Entities/
│   │   ├── BaseEntity.cs
│   │   ├── Company.cs
│   │   ├── Store.cs
│   │   ├── Employee.cs
│   │   ├── Client.cs
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Tag.cs
│   │   ├── ShoppingCart.cs
│   │   └── Sale.cs
│   ├── Enums/
│   │   └── Enums.cs         ← ContractType, EmployeeRole, PaymentMethod, SaleUnit
│   └── Interfaces/
│       └── IRepositories.cs ← Contratos de repositório
│
├── Infrastructure/          ← Implementações em memória dos repositórios
│   └── Repositories/
│       ├── InMemoryRepository.cs   (genérico abstrato)
│       └── ConcreteRepositories.cs (Employee, Client, Product, Sale, etc.)
│
├── Business/                ← Regras de negócio e serviços de aplicação
│   ├── Services/
│   │   ├── EmployeeService.cs
│   │   ├── ClientService.cs
│   │   ├── ProductService.cs
│   │   ├── CategoryService.cs
│   │   ├── TagService.cs
│   │   ├── StoreService.cs
│   │   ├── CartService.cs
│   │   └── SaleService.cs
│   └── AppContext.cs        ← Composição de dependências + dados iniciais (Seed)
│
├── ConsoleApp/              ← Interface de usuário (console)
│   ├── Helpers/
│   │   └── UI.cs            ← Utilitários de I/O com validação
│   ├── Menus/
│   │   ├── StoreMenu.cs
│   │   ├── EmployeeMenu.cs
│   │   ├── ClientMenu.cs
│   │   ├── CategoryTagMenus.cs
│   │   ├── ProductMenu.cs
│   │   └── SaleMenu.cs      ← PDV presencial + online + relatório gerencial
│   └── Program.cs           ← Ponto de entrada
│
├── Tests/                   ← Testes unitários XUnit
│   ├── EmployeeTests.cs
│   ├── ProductTests.cs
│   ├── CartAndSaleTests.cs
│   └── ClientTests.cs
│
└── NaturalStore.sln
```

---

## Requisitos Atendidos

| Requisito | Status |
|-----------|--------|
| Classes: Empresa, Loja, Funcionário, Cliente, Produto, Categoria, Tag, CarrinhoDeCompras | ✅ |
| CRUD completo para todas as entidades | ✅ |
| Funcionário com **múltiplas atribuições** (caixa, repositor, gerente, entregador) | ✅ |
| Cadastro de cliente online (login/senha com hash SHA-256) | ✅ |
| Gerente pode cadastrar clientes | ✅ |
| Venda presencial com seleção de caixa | ✅ |
| Venda online com carrinho de compras | ✅ |
| Pagamento: crédito, débito (online+presencial), dinheiro (só presencial) | ✅ |
| Histórico de compras do cliente | ✅ |
| Relatório de vendas por ano e mês (gerencial) | ✅ |
| Validação de campos na interface console | ✅ |
| Menus de navegação no console | ✅ |
| Dados gerenciados em memória (sem ORM) | ✅ |
| Projeto de testes unitários XUnit | ✅ |
| DDD + Clean Architecture | ✅ |
| Dados iniciais (Seed): 2 lojas, 7 funcionários, 4 produtos | ✅ |

---

## 🚀 Como Executar

### Pré-requisito
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Rodar o console
```bash
cd NaturalStore
dotnet run --project ConsoleApp
```

### Rodar os testes
```bash
dotnet test Tests
```

### Compilar a solução completa
```bash
dotnet build NaturalStore.sln
```

---

## 👤 Login de teste (seed)
- **Login:** `joao`  
- **Senha:** `123456`

---

## 🏗️ Arquitetura

```
ConsoleApp  →  Business (Services)  →  Domain (Entities + Interfaces)
                     ↑
              Infrastructure (Repositories)
```

- **Domain**: zero dependências externas. Contém as entidades ricas e interfaces.
- **Infrastructure**: implementa os repositórios em memória.
- **Business**: orquestra as regras de negócio usando os repositórios via interface.
- **ConsoleApp**: apenas UI — chama os serviços, nunca acessa repositórios diretamente.
