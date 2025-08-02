import { Component, inject, input } from '@angular/core';
import { MatCard, MatCardContent, MatCardTitle } from '@angular/material/card';
import { ExerciseDto } from '../../../dtos/exercise.dto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-exercise-card',
  imports: [MatCard, MatCardContent, MatCardTitle],
  templateUrl: './exercise-card.component.html',
  styleUrls: ['./exercise-card.component.scss']
})
export class ExerciseCardComponent {
  exercise = input<ExerciseDto>();

  private router = inject(Router);

  exercisePreview() {
    this.router.navigate([`app/exercises/preview/${this.exercise()?.id}`]);
  }
}
