import Common from "./common";
const { Create, Update, Delete, Detail, Dot, Export} = Common;
const GroupName = "Tasks";

const GroupNameTasks = GroupName + Dot + 'Task';

const Tasks = {
  Default: GroupNameTasks,
  Create: GroupNameTasks + Dot + Create,
  Update: GroupNameTasks + Dot + Update,
  Delete: GroupNameTasks + Dot + Delete,
  Detail: GroupNameTasks + Dot + Detail,
  Export: GroupNameTasks + Dot + Export,
};

export default {
  GroupName,
  Tasks,
};