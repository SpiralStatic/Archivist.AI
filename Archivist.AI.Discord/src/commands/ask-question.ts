import { archivistApiEndpoint } from '../../config.json';
import { ChatInputCommandInteraction, PermissionFlagsBits, SlashCommandBuilder } from "discord.js";
import { request } from "undici";
import { isAdmin, isOwner } from '../roles';

const name = 'ask-question';

const data = new SlashCommandBuilder()
  .setName(name)
  .setDescription('Ask a question of the archivist')
  .addStringOption(option =>
    option.setName('input')
      .setDescription('The question to ask')
      .setRequired(true)
      .setMaxLength(2000))
  .setDefaultMemberPermissions(PermissionFlagsBits.SendMessages);;

const execute = async (interaction: ChatInputCommandInteraction) => {
  const chatInput = interaction.options.getString('input');

  if (!chatInput) {
    console.error(`Invalid chat input`);
    return;
  }

  await interaction.deferReply();

  const response = await request(archivistApiEndpoint + '/api/chat', { method: 'POST', body: chatInput });

  if (response.statusCode === 200) {
    await interaction.reply('Your story has been added');
  }
  else {
    console.error(`Error with chat request`, response);
    await interaction.reply('Sorry, the archivist was unable to look that up for you');
  }
}

export const askQuestion = {
  data,
  execute
}