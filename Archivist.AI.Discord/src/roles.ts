import { GuildMemberRoleManager, Interaction } from "discord.js";

const ManagementPermission = "MANAGEMENT"
const ChatPermission = "CHAT";
const ArchivePermission = "ARCHIVE"

const AdminRole = "ADMIN";

const isAdmin = (interaction: Interaction) => {
  const roles = interaction.member?.roles as GuildMemberRoleManager;

  return roles.cache.some(role => role.name === 'Admin'
    || interaction.guild?.ownerId == interaction.member?.user.id);
}