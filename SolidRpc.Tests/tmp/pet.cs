
public interface pet {
  void addPet();
  void updatePet();
  findPetsByStatusArgs findPetsByStatus();
  findPetsByTagsArgs findPetsByTags();
  void deletePet();
  getPetByIdArgs getPetById();
  void updatePetWithForm();
  uploadFileArgs uploadFile();
}