export type CreateEditPlanDto = {
  name: string;
  description: string | null;
  exercisesIds: number[];
};
