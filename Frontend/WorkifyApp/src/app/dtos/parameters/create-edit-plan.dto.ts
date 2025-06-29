export interface CreateEditExerciseDto {
  name: string;
  description: string | null;
  exercisesIds: number[];
}
