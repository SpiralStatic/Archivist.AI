import { Client, Events, GatewayIntentBits } from 'discord.js';
import { token } from './config.json';

const client = new Client({ intents: [GatewayIntentBits.Guilds] });

client.once(Events.ClientReady, c => {
  console.log(`Ready! Logged in as ${c.user.tag}`);
});

client.login(token);