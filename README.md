# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)



 Resumen: ¿Blacklist sí o no?

  ┌─────────────────┬──────────────────────────────────────────────────┐
  │     Aspecto     │                  Recomendación                   │
  ├─────────────────┼──────────────────────────────────────────────────┤
  │ ¿Implementarlo? │ ✅ Sí, es útil para seguridad                    │
  ├─────────────────┼──────────────────────────────────────────────────┤
  │ ¿Hazlo bien?    │ ✅ Con persistencia y caché                      │
  ├─────────────────┼──────────────────────────────────────────────────┤
  │ ¿Dónde?         │ En el gateway (Ocelot) es el lugar ideal         │
  ├─────────────────┼──────────────────────────────────────────────────┤
  │ ¿Alternativas?  │ WAF externo (ModSecurity), Azure WAF, Cloudflare │
  └─────────────────┴──────────────────────────────────────────────────┘

  Lo que acabo de crear es:
  - ✅ Bloquea IPs configurables
  - ✅ Bloquea clientes por X-ClientId
  - ✅ Cargable desde appsettings.json
  - ✅ Fácil de actualizar en runtime
  Para producción, considera:
  1. Persistir en Redis o DB (en lugar de HashSet en memoria)
  2. Agregar logging de intentos bloqueados
  3. Implementar white-list también (whitelist + blacklist)
  4. Auto-bloqueo por comportamiento sospechoso (múltiples 401s, etc.)







    # A través del gateway:
  curl https://localhost:7090/movies/Categories