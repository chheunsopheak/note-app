<script setup>
import { ref, computed, onMounted } from "vue";
const notes = ref([]);
const searchQuery = ref("");
const isAddModalOpen = ref(false);
const isEditModalOpen = ref(false);
const isDetailModalOpen = ref(false);
const isDeleteModalOpen = ref(false);
const selectedNote = ref({});
const editableNote = ref({ id: null, title: "", content: "" });
const titleError = ref("");

// Fetch All Notes
const fetchNotes = async () => {
    try {
        const response = await fetch("https://localhost:5000/api/notes");
        if (!response.ok) throw new Error("Failed to fetch notes");
        notes.value = await response.json();
    } catch (error) {
        console.error("Error fetching notes:", error);
    }
};

// Fetch Note by ID
const fetchNoteById = async (id) => {
    try {
        const response = await fetch(`https://localhost:5000/api/note/${id}`);
        if (!response.ok) throw new Error("Failed to fetch note");

        const result = await response.json();
        selectedNote.value = result.data;
        isDetailModalOpen.value = true;
    } catch (error) {
        console.error("Error fetching note:", error);
    }
};

// Add or Update Note
const saveNote = async () => {
    try {
        if (!editableNote.value.title || editableNote.value.title.trim() === "") {
            titleError.value = "Title is required.";
            return;
        }

        titleError.value = "";

        let url = "https://localhost:5000/api/note";
        let method = "POST";

        if (editableNote.value.id) {
            url = `https://localhost:5000/api/note/${editableNote.value.id}`;
            method = "PUT";
        }

        const response = await fetch(url, {
            method: method,
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ title: editableNote.value.title, content: editableNote.value.content })
        });

        if (!response.ok) throw new Error("Failed to save note");

        await fetchNotes();
        closeModals();
    } catch (error) {
        console.error("Error saving note:", error);
    }
};

// Delete Note
const deleteNote = async () => {
    try {
        const response = await fetch(`https://localhost:5000/api/note/${selectedNote.value.id}`, {
            method: "DELETE"
        });

        if (!response.ok) throw new Error("Failed to delete note");

        await fetchNotes();
        closeModals();
    } catch (error) {
        console.error("Error deleting note:", error);
    }
};

// Format Date
const formatDate = (dateString) => {
    if (!dateString) return "N/A";
    return new Date(dateString).toLocaleDateString("en-US", {
        year: "numeric",
        month: "short",
        day: "numeric"
    });
};

// Open Modals
const openAddModal = () => { editableNote.value = { id: null, title: "", content: "" }; isAddModalOpen.value = true; };
const openEditModal = (note) => { editableNote.value = { ...note }; isEditModalOpen.value = true; };
const openDetailModal = (note) => { fetchNoteById(note.id); };
const openDeleteModal = (note) => { selectedNote.value = note; isDeleteModalOpen.value = true; };

// Close Modals
const closeModals = () => { isAddModalOpen.value = isEditModalOpen.value = isDetailModalOpen.value = isDeleteModalOpen.value = false; };

// Search Filter
const filteredNotes = computed(() => {
    if (!searchQuery.value) return notes.value;
    return notes.value.filter(note =>
        note.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
        note.content.toLowerCase().includes(searchQuery.value.toLowerCase())
    );
});

// Fetch Notes on Page Load
onMounted(fetchNotes);
</script>


<template>
    <div class="w-[900px] mx-auto p-10 bg-gray-900 text-white">
        <!-- Header -->
        <div class="flex justify-between items-center mb-4">
            <h2 class="text-xl font-semibold">Daily Journal Notes</h2>
            <button @click="openAddModal" class="px-5 py-3 bg-blue-600 text-sm rounded-lg hover:bg-blue-700">+ Add
                New</button>
        </div>

        <!-- Search Bar -->
        <input v-model="searchQuery" placeholder="Search notes..."
            class="w-full px-4 py-3 mb-4 text-white bg-gray-800 rounded-lg border border-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500 placeholder-gray-400" />

        <!-- Notes Table -->
        <div v-if="filteredNotes.length > 0" class="overflow-x-auto">
            <table class="w-full border-collapse border border-gray-700 text-white">
                <thead class="bg-gray-800 text-righ">
                    <tr>
                        <th class="border border-gray-700 px-4 py-2">ID</th>
                        <th class="border border-gray-700 px-4 py-2">Title</th>
                        <th class="border border-gray-700 px-4 py-2">Created At</th>
                        <th class="border border-gray-700 px-4 py-2">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="note in filteredNotes" :key="note.id" class="bg-gray-900 hover:bg-gray-800 text-right">
                        <td class="border border-gray-700 px-4 py-2 text-center">{{ note.id }}</td>
                        <td class="border border-gray-700 px-4 py-2 text-left">{{ note.title }}</td>
                        <td class="border border-gray-700 px-4 py-2 text-center">{{ formatDate(note.createdAt) }}</td>
                        <td class="border border-gray-700 px-4 py-2 text-center space-x-2">
                            <button @click.stop="openDetailModal(note)" class="text-blue-400 hover:underline">
                                <i class="fa-solid fa-eye"></i>
                            </button>
                            <button @click.stop="openEditModal(note)" class=" hover:underline">
                                <i class="fa-solid fa-pen-to-square"></i>
                            </button>
                            <button @click.stop="openDeleteModal(note)" class="text-red-400 hover:underline">
                                <i class="fa-solid fa-trash-can"></i></button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <p v-else class="text-gray-500 text-center">No notes found.</p>

        <!-- Add/Edit Note -->
        <div v-if="isEditModalOpen || isAddModalOpen"
            class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
            <div class="bg-gray-900 p-6 rounded-lg shadow-lg w-96 md:w-1/2 lg:w-1/2">
                <h2 class="text-lg text-left  font-semibold mb-4 ">{{ isAddModalOpen ? "Add New Note" : "Edit Note"
                    }}</h2>
                <label class="block text-left text-gray-300 pr-10">Title</label>
                <input v-model="editableNote.title" type="text"
                    class="w-full px-3 py-2 mb-3 text-white bg-gray-800 rounded-lg border border-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
                    placeholder="Enter title" required />
                <p v-if="titleError" class="text-red-400 text-left text-sm mt-1">{{ (isEditModalOpen || isAddModalOpen)
                    ? titleError : "" }}</p>

                <label class="block text-left text-gray-300">Content</label>
                <textarea v-model="editableNote.content" rows="4" placeholder="Enter Content" id="content"
                    class="w-full px-3 py-2 text-white bg-gray-800 rounded-lg border border-gray-700 focus:outline-none focus:ring-2 focus:ring-blue-500"></textarea>

                <div class="flex justify-end space-x-2 mt-4">
                    <button @click="closeModals"
                        class="px-4 py-2 bg-gray-600 rounded-lg hover:bg-gray-700">Cancel</button>
                    <button @click="saveNote" class="px-4 py-2 bg-blue-600 rounded-lg hover:bg-blue-700">
                        {{ isEditModalOpen ? "Update" : "Add" }}
                    </button>
                </div>
            </div>
        </div>

        <!-- View Note Detail -->

        <div v-if="isDetailModalOpen" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
            <div class="bg-gray-900 p-6 rounded-lg shadow-lg w-11/12 md:w-1/2 lg:w-1/3">
                <h2 class="text-lg text-center text-green-500 font-semibold mb-6">Note Details</h2>

                <div class="space-y-4 text-left">
                    <div>
                        <label class="block text-left text-sm font-medium text-gray-400 mb-1">Title</label>
                        <p class="text-white text-base bg-gray-800 p-3 rounded-lg">{{ selectedNote.title }}</p>
                    </div>

                    <div>
                        <label class="block text-left text-sm font-medium text-gray-400 mb-1">Content</label>
                        <p class="text-white text-base bg-gray-800 p-3 rounded-lg whitespace-pre-wrap">{{
                            selectedNote.content }}</p>
                    </div>

                    <div>
                        <label class="block text-sm text-left font-medium text-gray-400 mb-1">Created At</label>
                        <p class="text-white text-base bg-gray-800 p-3 rounded-lg">{{ formatDate(selectedNote.createdAt)
                            }}</p>
                    </div>

                    <div>
                        <label class="block text-sm font-medium text-gray-400 mb-1">Updated At</label>
                        <p class="text-white  text-base bg-gray-800 p-3 rounded-lg">{{
                            formatDate(selectedNote.updatedAt)
                            }}</p>
                    </div>
                </div>

                <div class="flex justify-end mt-6">
                    <button @click="closeModals"
                        class="px-4 py-2 bg-red-600 text-v rounded-lg hover:bg-gray-700 transition duration-200">
                        Close
                    </button>
                </div>
            </div>
        </div>

        <!-- Delete Confirm -->
        <div v-if="isDeleteModalOpen" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
            <div class="bg-gray-900 p-6 rounded-lg shadow-lg w-96 text-center">
                <h2 class="text-lg font-semibold mb-4">Are you sure?</h2>
                <p class="text-gray-400 mb-4">Do you really to delete this?<br> This process cannot be undone.</p>
                <div class="flex justify-center space-x-2">
                    <button @click="isDeleteModalOpen = false"
                        class="px-4 py-2 bg-gray-600 rounded-lg hover:bg-gray-700">Cancel</button>
                    <button @click="deleteNote" class="px-4 py-2 bg-red-600 rounded-lg hover:bg-red-700">Delete</button>
                </div>
            </div>
        </div>
    </div>
</template>
