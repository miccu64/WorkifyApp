import { Component, inject, OnInit } from '@angular/core';
import { ExerciseDto } from '../../dtos/exercise.dto';
import { ListComponent } from '../../layout/list/list.component';
import { WorkoutService } from '../../services/workout.service';
import { ExerciseCardComponent } from '../subcomponents/exercise-card/exercise-card.component';

@Component({
  selector: 'app-exercises-list',
  imports: [ExerciseCardComponent, ListComponent],
  templateUrl: './exercises-list.component.html',
  styleUrl: './exercises-list.component.scss'
})
export class ExercisesListComponent implements OnInit {
  exercises: ExerciseDto[] = [];

  private workoutService = inject(WorkoutService);

  ngOnInit(): void {
    this.exercises = this.workoutService.exercises;
  }

  openCreateForm(): void {
    // TODO:
  }
}
