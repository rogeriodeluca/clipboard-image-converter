# ClipboardImageConverter

ClipboardImageConverter é um pequeno utilitário para Windows que monitora a área de transferência e, quando detecta uma URL que aponta para uma imagem, baixa essa imagem, converte para JPG e salva automaticamente na pasta de destino configurada pelo usuário.

O projeto nasceu de uma necessidade prática: agilizar o cadastro de produtos em sistemas e marketplaces, evitando o processo manual de baixar imagens em WebP, AVIF ou outros formatos e depois convertê-las para JPG.

## Funcionalidades

- Monitora alterações na área de transferência do Windows enquanto o aplicativo está aberto.
- Detecta URLs HTTP e HTTPS.
- Verifica o `Content-Type`, quando informado, e tenta decodificar o conteúdo como imagem; não depende apenas da extensão da URL.
- Suporta formatos como AVIF, WebP, PNG e JPEG por meio do Magick.NET.
- Converte imagens para JPG e aplica fundo branco quando há transparência.
- Tenta manter o nome original da imagem a partir da URL, ajustando caracteres inválidos.
- Evita sobrescritas, criando nomes como `arquivo.jpg`, `arquivo (1).jpg` e `arquivo (2).jpg`.
- Permite escolher a pasta de destino e persiste a última escolha para as próximas execuções.
- Utiliza a pasta Downloads real do usuário do Windows quando não há configuração válida, respeitando redirecionamentos.

A interface mostra o status do monitoramento, a última URL detectada, o resultado da operação, o caminho do último JPG salvo e a pasta atual de destino.

## Tecnologias

- C# e .NET 10, com destino `net10.0-windows`.
- Windows Forms (WinForms).
- `HttpClient` para download das imagens.
- APIs do Windows para notificações do Clipboard e consulta à Known Folder Downloads (`SHGetKnownFolderPath`).
- `Magick.NET-Q8-AnyCPU` 15.0.0 para processamento de imagens.
- `System.Text.Json` para persistência das configurações.

## Requisitos

- Windows.
- Para desenvolvimento: .NET 10 SDK e Git para clonar o repositório.
- Para executar uma publicação *framework-dependent*: .NET 10 Desktop Runtime compatível com a arquitetura da publicação. Consulte a [documentação de instalação do .NET no Windows](https://learn.microsoft.com/en-us/dotnet/core/install/windows).
- Acesso à internet para baixar imagens de URLs remotas e permissão de gravação na pasta de destino.

## Executar para desenvolvimento

No Windows, execute:

```powershell
git clone https://github.com/rogeriodeluca/clipboard-image-converter.git
cd clipboard-image-converter\ClipboardImageConverter
dotnet restore
dotnet build
dotnet run
```

## Executar uma versão publicada

Em uma versão publicada, abra `ClipboardImageConverter.exe`. Mantenha os demais arquivos da publicação junto ao executável, conforme o pacote distribuído.

Se a publicação for *framework-dependent*, instale o .NET 10 Desktop Runtime antes de executar. A presença do arquivo `.exe` por si só não significa que o runtime esteja incluído.

## Uso

1. Abra o aplicativo.
2. Confirme a pasta de destino exibida ou clique em **Alterar pasta...** para escolher outra.
3. Copie para a área de transferência uma URL que retorne diretamente o conteúdo de uma imagem.
4. O programa detectará a URL e tentará baixar a imagem.
5. Se o conteúdo for uma imagem válida, ele será convertido automaticamente para JPG e salvo na pasta de destino.
6. Confira o resultado e o caminho do arquivo na interface.

A URL deve retornar diretamente o conteúdo da imagem. Uma página HTML que apenas exibe uma imagem não é suficiente. O aplicativo processa uma imagem por vez; alterações no Clipboard durante esse processamento são ignoradas.

## Configuração da pasta de destino

A pasta escolhida é salva em um arquivo JSON dentro da área de dados locais do usuário do Windows, obtida dinamicamente pela API `Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)`:

```text
%LOCALAPPDATA%\ClipboardImageConverter\settings.json
```

Ao iniciar, o aplicativo restaura a pasta configurada. Se ela não existir, tenta criá-la e verifica se é possível gravar nela. Se a configuração não existir, estiver vazia ou inválida, não puder ser lida, ou a pasta configurada não puder ser utilizada, o aplicativo usa a pasta Downloads real do usuário.

A pasta Downloads é consultada pela API de Known Folders do Windows, incluindo casos em que foi redirecionada para outro disco ou local.

Ao confirmar uma nova pasta, a configuração é salva e os próximos salvamentos usam esse destino sem reiniciar. Cancelar o seletor mantém a escolha anterior. Se não for possível salvar a configuração, o aplicativo informa o erro e mantém o destino anterior.

## Privacidade

O aplicativo monitora o Clipboard e converte as imagens localmente. O código não implementa envio de dados para servidores próprios; ele acessa as URLs de imagem copiadas pelo usuário para realizar o download. As requisições são enviadas aos servidores dessas URLs, incluindo eventuais redirecionamentos HTTP.

## Possíveis melhorias futuras

Possibilidades para evolução, sem compromisso de implementação:

- System Tray opcional e inicialização automática com o Windows.
- Histórico das imagens processadas.
- Instalação via setup/MSIX e opções de publicação self-contained ou em arquivo único.
- Outras opções de formato de saída.

## Licença

Este projeto utiliza a licença MIT. Consulte o arquivo [LICENSE](LICENSE).
