import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ArticleService } from '../services/article.service';


@Component({
  selector: 'app-edit-article-dialog',  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatSnackBarModule
  ],
  templateUrl: './edit-article-dialog.component.html',
  styleUrls: ['./edit-article-dialog.component.css']
})
export class EditArticleDialogComponent {
  editForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<EditArticleDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { article: any },
    private fb: FormBuilder,
    private articleService: ArticleService,
    private snackBar: MatSnackBar
  ) {
    this.editForm = this.fb.group({
      nom: [data.article.nom, Validators.required],
      prixHT: [data.article.prixHT, [Validators.required, Validators.min(0.01)]],
      dlc: [data.article.dlc],
      typeVente: [data.article.typeVente],
      packaging: [data.article.packaging]
    });
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onSave(): void {
    const updatedArticle = {
      reference: this.data.article.reference,
      nom: this.editForm.value.nom,
      prixHT: this.editForm.value.prixHT,
      dlc: this.editForm.value.dlc,
      typeVente: this.editForm.value.typeVente,
      packaging: this.editForm.value.packaging
    };
    this.articleService.updateArticle(this.data.article.reference, updatedArticle).subscribe({
      next: () => {
        this.snackBar.open('Article modifié', 'OK', { duration: 2000 });
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.snackBar.open(err.message, 'Fermer', { duration: 4000 });
      }
    });
  }
}