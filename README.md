# Teste-WorkerService

<b>Segue as instruções para instalar o serviço.
</b>

#1 Publique o projeto.

#2 - executar o CMD como administrador e seguir os comandos -->

//instalação do serviço
2.1-> sc.exe create nomeDoServiço binpath= local\release\serviço

//para INICIAR o serviço inatalado
2.2-> sc.exe start nomeDoServiço 

//para PARAR o serviço instalado
2.3-> sc.exe stop nomeDoServiço 

//para EXCLUIR o serviço instalado
2.4->sc.exe delete nomeDoServiço


OBS.: pasta de log do sistema será criado em "D:\\logWorkerService.txt";
