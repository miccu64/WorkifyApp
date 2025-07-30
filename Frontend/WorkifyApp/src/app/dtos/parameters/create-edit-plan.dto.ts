export interface CreateEditPlanDto {
  name: string;
  description: string | null;
  exercisesIds: number[];
}
