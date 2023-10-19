import { archivistApiEndpoint } from '../../config.json';
import { ChatInputCommandInteraction, PermissionFlagsBits, SlashCommandBuilder } from "discord.js";
import { request } from "undici";
import { isAdmin, isArchivist } from '../roles';

const name = 'add-knowledge';

const data = new SlashCommandBuilder()
  .setName(name)
  .setDescription('Add to knowledge to the library')
  .addStringOption(option =>
    option.setName('input')
      .setDescription('The story to be shared')
      .setRequired(true)
      .setMaxLength(2000))
  .setDefaultMemberPermissions(PermissionFlagsBits.SendMessages);

const execute = async (interaction: ChatInputCommandInteraction) => {
  if (!isAdmin(interaction) || !isArchivist(interaction)) {
    console.error('Invalid permissions');
    await interaction.reply('Sorry, you do not have permissions to do that');
    return;
  }

  const chatInput = interaction.options.getString('input');

  if (!chatInput) {
    console.error('Invalid chat input');
    return;
  }

  await interaction.deferReply();

  const userId = interaction.user.id;
  const guildId = interaction.guildId;

  if (!userId) {
    console.error('Missing userId value');
    return;
  }

  if (!guildId) {
    console.error('Missing guildId value');
    return;
  }

  const queryParams = new URLSearchParams({
    userId: btoa(userId),
    guildId: btoa(guildId)
  });

  const url = new URL('/api/embeddings?' + queryParams.toString(), archivistApiEndpoint);

  const response = await request(url, { method: 'POST', body: chatInput });

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