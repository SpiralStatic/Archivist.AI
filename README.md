# Archivist.AI

## TODOs

* Better storage mechanism for embeddings
* Per Server (Guild) instances of the discord bot
* Management discord bot commands
  * How many embeddings are there currently
  * Delete embeddings
* Add dates to embeddings (Fictional vs Real)
* Testing
* Performance testing with bot
* Authorization features
  * User vs admin

## Out there
* Profanity feature
* AI Image creation
* Image Attachment feature

# Setup

## How to Build

### API
* Perform the following command `dotnet build` in the Archivist.AI.API project folder

### Discord
* Ensure you have pnpm installed. See https://pnpm.io/installation#using-npm
* Run the following command `pnpm build` in the Archivist.AI.Discord project folder

## How to run
### API
Note: If using the command line the following command can set a secret: `dotnet user-secrets set "KEY" "VALUE"`
* Update your dotnet user-secrets with the following secrets set:
  * OpenAIServiceOptions:ApiKey // This is your OpenAI Api Key that is generated within your OpenAI account
* Perform the following command `dotnet run` in the Archivist.AI.API project folder

### Discord
* Create a config file with the following parameters:
```json
{
  "token": "", // Discord token
  "clientId": "", // Discord client ID
  "guildId": "", // Discord Guild ID
  "archivistApiEndpoint": "http://localhost:5298" // Or whatever host your API is running on
}
```
* Ensure you have node (v20 and above). See https://nodejs.org/en
* Perform the following command `node .` in the Archivist.AI.Discord project folder
