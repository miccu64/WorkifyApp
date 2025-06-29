import { BodyPartEnum } from '../enums/body-part.enum';

export interface CreateEditExerciseDto {
  name: string;
  bodyPart: BodyPartEnum;
  description: string | null;
}
