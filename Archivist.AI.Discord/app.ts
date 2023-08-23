import { token, apiEndpoint } from './config.json';
import { addKnowledge } from './commands/addKnowledge';
import { request } from 'undici';
import { ChatInputCommandInteraction, Client, Events, GatewayIntentBits } from 'discord.js';

const client = new Client({ intents: [GatewayIntentBits.Guilds] });

client.once(Events.ClientReady, c => {
  console.log(`Ready! Logged in as ${c.user.tag}`);
});

client.on('interactionCreate', async interaction => {
  if (!interaction.isChatInputCommand()) return;

  const command = interaction as ChatInputCommandInteraction;

  if (command.commandName === addKnowledge) {
    const text = command.options.getString('input');
    await interaction.deferReply();
    await request(apiEndpoint + '/api/embeddings', { method: 'POST', body: text });
  }
});

client.login(token);
