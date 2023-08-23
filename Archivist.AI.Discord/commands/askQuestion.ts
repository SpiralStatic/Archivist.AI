import { ChatInputCommandInteraction, SlashCommandBuilder } from "discord.js";

export const askQuestion = 'askQuestion';

const data = new SlashCommandBuilder()
  .setName(askQuestion)
  .setDescription('Ask a question of the archivist')
  .addStringOption(option =>
    option.setName('input')
      .setDescription('The question to ask')
      .setRequired(true)
      .setMaxLength(2000));

module.exports = {
	data,
	async execute(interaction: ChatInputCommandInteraction) {
		await interaction.reply({ content: 'Asking the archivist', ephemeral: true });
	},
};