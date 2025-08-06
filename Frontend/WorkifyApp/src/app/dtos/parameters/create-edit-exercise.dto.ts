import { BodyPartEnum } from '../enums/body-part.enum';

export type CreateEditExerciseDto = {
  name: string;
  bodyPart: BodyPartEnum;
  description: string | null;
};
