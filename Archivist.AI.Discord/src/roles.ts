import { Guild, GuildMemberRoleManager, Interaction, PermissionsBitField } from "discord.js";

const AdminRole = "Admin";
const ArchiveRole = "Archivist"

export const isOwner = (interaction: Interaction) => {
  return interaction.guild?.ownerId == interaction.member?.user.id;
}

export const isAdmin = (interaction: Interaction) => {
  const roles = interaction.member?.roles as GuildMemberRoleManager;

  return roles.cache.some(role => role.name === AdminRole);
}

export const isArchivist = (interaction: Interaction) => {
  const roles = interaction.member?.roles as GuildMemberRoleManager;

  return roles.cache.some(role => role.name === ArchiveRole);
}

const setDefaultPermissions = (guild: Guild) => {
  guild.roles.everyone.setPermissions([PermissionsBitField.Flags.SendMessages, PermissionsBitField.Flags.ViewChannel]);
}