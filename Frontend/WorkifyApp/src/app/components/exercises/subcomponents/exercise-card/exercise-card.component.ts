import { ChangeDetectionStrategy, Component, inject, input } from '@angular/core';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { Router } from '@angular/router';
import { BodyPartEnum } from '../../../../dtos/enums/body-part.enum';
import { ExerciseDto } from '../../../../dtos/exercise.dto';

@Component({
  selector: 'app-exercise-card',
  imports: [MatCard, MatCardContent, MatCardTitle],
  templateUrl: './exercise-card.component.html',
  styleUrls: ['./exercise-card.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExerciseCardComponent {
  readonly exercise = input<ExerciseDto>();

  get bodyPartName(): string {
    return BodyPartEnum[this.exercise()?.bodyPart ?? BodyPartEnum.Other];
  }

  private router = inject(Router);

  exercisePreview(): void {
    this.router.navigate([`app/exercises/preview/${this.exercise()?.id}`]);
  }
}
