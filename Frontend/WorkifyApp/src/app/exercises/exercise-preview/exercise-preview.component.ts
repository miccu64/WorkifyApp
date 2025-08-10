import { CommonModule, Location } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { MatButtonModule, MatFabButton } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { BodyPartEnum } from '../../dtos/enums/body-part.enum';
import { ExerciseDto } from '../../dtos/exercise.dto';
import { InfoTableComponent } from '../../layout/info-table/info-table.component';
import { WorkoutService } from '../../services/workout.service';
import { CreateEditExerciseFormComponent } from '../subcomponents/create-edit-exercise/create-edit-exercise-form.component';
import { ListComponent } from '../../layout/list/list.component';
import { CreateEditStatFormComponent } from '../../stats/subcomponents/create-edit-stat/create-edit-stat-form.component';

@Component({
  selector: 'app-exercise-preview',
  imports: [InfoTableComponent, MatIcon, MatIconModule, MatFabButton, MatButtonModule, CommonModule, ListComponent],
  templateUrl: './exercise-preview.component.html',
  styleUrls: ['./exercise-preview.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ExercisePreviewComponent implements OnInit {
  exercise!: ExerciseDto;
  tableData!: Map<string, string>;

  disabledBtnText = this.exercise?.isCustom === false ? '' : 'This exercise is predefined and cannot be modified';

  private activatedRoute = inject(ActivatedRoute);
  private workoutService = inject(WorkoutService);
  private dialog = inject(MatDialog);
  private router = inject(Router);
  private location = inject(Location);
  private changeDetectorRef = inject(ChangeDetectorRef);

  ngOnInit(): void {
    const exerciseId = Number(this.activatedRoute.snapshot.paramMap.get('exerciseId'));
    this.refreshData(exerciseId);
  }

  editExercise(): void {
    const dialogRef = this.dialog.open(CreateEditExerciseFormComponent, { data: { exercise: this.exercise } });
    dialogRef.afterClosed().subscribe(() => {
      this.refreshData(this.exercise.id);
    });
  }

  async deleteExercise(): Promise<void> {
    if (confirm('Are you sure to delete exercise "' + this.exercise.name + '"?')) {
      await firstValueFrom(this.workoutService.deleteExercise(this.exercise.id));

      await this.workoutService.refreshPlansAndExercises();

      await this.router.navigate(['/app/exercises/list']);
    }
  }

  goBack(): void {
    this.location.back();
  }

  addStat(): void {
    const dialogRef = this.dialog.open(CreateEditStatFormComponent, { data: { exerciseId: this.exercise.id } });
    dialogRef.afterClosed().subscribe(() => {
      this.refreshData(this.exercise.id);
    });
  }

  private refreshData(exerciseId: number): void {
    const exercise = this.workoutService.exercises.find(e => e.id === exerciseId);
    if (!exercise) {
      throw new Error('Exercise not found');
    }

    this.exercise = exercise;
    this.tableData = new Map<string, string>([
      ['Body part', BodyPartEnum[exercise.bodyPart]],
      ['Description', exercise.description ?? '-']
    ]);

    this.changeDetectorRef.markForCheck();
  }
}
