import { archivistApiEndpoint } from '../../config.json';
import { ChatInputCommandInteraction, SlashCommandBuilder } from "discord.js";
import { request } from "undici";

const name = 'add-knowledge';

const data = new SlashCommandBuilder()
  .setName(name)
  .setDescription('Add to knowledge to the library')
  .addStringOption(option =>
    option.setName('input')
      .setDescription('The story to be shared')
      .setRequired(true)
      .setMaxLength(2000));

const execute = async (interaction: ChatInputCommandInteraction) => {
  const chatInput = interaction.options.getString('input');

  if (!chatInput) {
    console.error(`Invalid chat input`);
    return;
  }

  await interaction.deferReply();

  const response = await request(archivistApiEndpoint + '/api/embeddings', { method: 'POST', body: chatInput });

  if (response.statusCode === 200) {
    await interaction.reply('Your story has been added');
  }
  else {
    console.error(`Error with embedding request`, response);
    await interaction.reply('Sorry, the archivist was unable to record your story');
  }
}

export const addKnowledge = {
  data,
  execute
};