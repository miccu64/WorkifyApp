import { Component, inject, input } from '@angular/core';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { ExerciseDto } from '../../../dtos/exercise.dto';
import { Router } from '@angular/router';
import { BodyPartEnum } from '../../../dtos/enums/body-part.enum';

@Component({
  selector: 'app-exercise-card',
  imports: [MatCard, MatCardContent, MatCardTitle],
  templateUrl: './exercise-card.component.html',
  styleUrls: ['./exercise-card.component.scss']
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
