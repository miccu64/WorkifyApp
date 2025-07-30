import { Component, inject, OnInit } from '@angular/core';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCard, MatCardModule, MatCardTitle } from '@angular/material/card';
import { ExerciseDto } from '../../dtos/exercise.dto';
import { ExerciseCardComponent } from '../subcomponents/exercise-card/exercise-card.component';
import { WorkoutService } from '../../services/workout.service';

@Component({
  selector: 'app-exercises-list',
  imports: [MatCard, MatCardTitle, MatListModule, MatIconModule, MatButtonModule, MatCardModule, ExerciseCardComponent],
  templateUrl: './exercises-list.component.html',
  styleUrl: './exercises-list.component.scss'
})
export class ExercisesListComponent implements OnInit {
  exercises: ExerciseDto[] = [];

  private workoutService = inject(WorkoutService);

  ngOnInit(): void {
    this.exercises = this.workoutService.exercises;
  }
}
