import { Client, Events, GatewayIntentBits } from 'discord.js';
import { token } from './config.json';
import { addKnowledge } from './commands/addKnowledge';

const client = new Client({ intents: [GatewayIntentBits.Guilds] });
client.once(Events.ClientReady, c => {
  console.log(`Ready! Logged in as ${c.user.tag}`);
});

client.on('interactionCreate', async interaction => {
  if (!interaction.isChatInputCommand()) return;

  if (interaction.commandName === addKnowledge) {
    await interaction.reply('Pong!');
  }
});

client.login(token);