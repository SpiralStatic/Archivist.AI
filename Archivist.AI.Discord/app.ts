import { addKnowledge } from './commands/addKnowledge';
import { askQuestion } from './commands/askQuestion';
import { ChatInputCommandInteraction, Client, Events, GatewayIntentBits } from 'discord.js';
import { request } from 'undici';
import { token, apiEndpoint } from './config.json';

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
  else if (command.commandName === askQuestion) {
    const text = command.options.getString('input');
    await interaction.deferReply();
    await request(apiEndpoint + '/api/chat', { method: 'POST', body: text });
  }
  else {
    await interaction.reply('Command not recognised');
  }
});

client.login(token);
