import { ChangeDetectionStrategy, Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatCard, MatCardModule } from '@angular/material/card';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { firstValueFrom, Observable } from 'rxjs';
import { CreateEditStatDto } from '../../../../dtos/parameters/create-edit-stat.dto';
import { StatDto } from '../../../../dtos/stat.dto';
import { StatService } from '../../../../services/stat.service';

@Component({
  selector: 'app-create-edit-stat-form',
  templateUrl: './create-edit-stat-form.component.html',
  styleUrls: ['./create-edit-stat-form.component.scss'],
  providers: [provideNativeDateAdapter()],
  imports: [
    MatDatepickerModule,
    MatCard,
    MatCardModule,
    MatInputModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatButton,
    MatButtonModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CreateEditStatFormComponent implements OnInit {
  form!: FormGroup;

  private matDialogData = inject(MAT_DIALOG_DATA);

  stat: StatDto = this.matDialogData.stat;
  exerciseId: number = this.matDialogData.exerciseId;
  readonly maxDate = new Date();

  private get isEditMode(): boolean {
    return this.stat ? true : false;
  }

  private statService = inject(StatService);
  private formBuilder = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<CreateEditStatFormComponent>);

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      weight: [this.stat?.weight, [Validators.required, Validators.min(0)]],
      reps: [this.stat?.reps, [Validators.required, Validators.min(1), this.integerValidator]],
      time: [this.stat?.time ?? new Date(), [Validators.required]],
      note: [this.stat?.note]
    });

    if (this.isEditMode) {
      this.form.get('time')?.disable();
    }
  }

  async onSubmit(): Promise<void> {
    if (!this.form.valid) {
      return;
    }

    const parameters: CreateEditStatDto = {
      weight: this.form.get('weight')?.value,
      reps: this.form.get('reps')?.value,
      time: new Date(this.form.get('time')?.value),
      note: this.form.get('note')?.value
    };

    let request: Observable<number>;
    if (this.isEditMode) {
      request = this.statService.editStat(this.stat.id, parameters);
    } else {
      request = this.statService.createStat(this.exerciseId, parameters);
    }

    await firstValueFrom(request);

    this.dialogRef.close();
  }

  deleteStat(): void {
    if (confirm('Are you sure to delete selected stat?')) {
      this.statService.deleteStat(this.stat.id).subscribe(() => {
        this.dialogRef.close();
      });
    }
  }

  private integerValidator(control: FormControl): ValidationErrors | null {
    if (control.value) {
      const number = Number(control.value);
      if (!Number.isInteger(number)) {
        return { invalidNumber: true };
      }
    }

    return null;
  }
}
