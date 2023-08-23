import { addKnowledge } from "./add-knowledge";
import { askQuestion } from "./ask-question";

export const commands = {
    [addKnowledge.data.name]: addKnowledge,
    [askQuestion.data.name]: askQuestion
}