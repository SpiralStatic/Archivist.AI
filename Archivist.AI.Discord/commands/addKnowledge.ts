import { ChatInputCommandInteraction, SlashCommandBuilder } from "discord.js";

const data = new SlashCommandBuilder()
  .setName('addKnowledge')
  .setDescription('Add to knowledge to the library')
  .addStringOption(option =>
    option.setName('input')
      .setDescription('The story to be shared')
      .setRequired(true)
      .setMaxLength(2000));

module.exports = {
	data,
	async execute(interaction: ChatInputCommandInteraction) {
		await interaction.reply('Pong!');
	},
};