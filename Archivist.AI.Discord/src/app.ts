import { ChatInputCommandInteraction, Client, Events, GatewayIntentBits } from 'discord.js';
import { token } from '../config.json';
import { commands }  from './commands/index';

const client = new Client({ intents: [GatewayIntentBits.Guilds] });

client.once(Events.ClientReady, c => {
  console.log(`Ready! Logged in as ${c.user.tag}`);
});

client.on('interactionCreate', async interaction => {
  if (!interaction.isChatInputCommand()) return;

  const chatInputInteraction = interaction as ChatInputCommandInteraction;

  const command = commands[interaction.commandName];

  if (!command) {
    console.error(`No command matching ${chatInputInteraction.commandName} was found.`);
    await chatInputInteraction.reply({ content: 'Command not recognised', ephemeral: true });
    return;
  }

  try {
    command.execute(chatInputInteraction);
  }
  catch (error) {
    console.error(error);
    return;
  }
});

client.login(token);
