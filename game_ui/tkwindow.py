import tkinter
import tkinter.messagebox
import customtkinter

import sys
# Setting path to access the files in parent directory
sys.path.append('../')

import game_data

customtkinter.set_appearance_mode("System")  # Modes: "System" (standard), "Dark", "Light"
customtkinter.set_default_color_theme("blue")  # Themes: "blue" (standard), "green", "dark-blue"


class App(customtkinter.CTk):

    WIDTH = 660
    HEIGHT = 650

    def __init__(self):
        super().__init__()

        self.title("Wordle Remastered")
        self.geometry(f"{App.WIDTH}x{App.HEIGHT}")
        # Set fixed size
        self.resizable(False, False)
        self.protocol("WM_DELETE_WINDOW", self.on_closing)  # call .on_closing() when app gets closed

        # ============ create two frames ============

        # configure grid layout (2x1)
        self.grid_columnconfigure(1, weight=1)
        self.grid_rowconfigure(0, weight=1)

        self.frame_left = customtkinter.CTkFrame(master=self,
                                                 width=180,
                                                 corner_radius=0)
        self.frame_left.grid(row=0, column=0, sticky="nswe")

        self.frame_right = customtkinter.CTkFrame(master=self)
        self.frame_right.grid(row=0, column=1, sticky="nswe", padx=20, pady=20)

        # ============ frame_left ============

        # configure grid layout (1x11)
        self.frame_left.grid_rowconfigure(0, minsize=10)   # empty row with minsize as spacing
        self.frame_left.grid_rowconfigure(5, weight=1)  # empty row as spacing
        self.frame_left.grid_rowconfigure(8, minsize=20)    # empty row with minsize as spacing
        self.frame_left.grid_rowconfigure(11, minsize=10)  # empty row with minsize as spacing

        self.label_1 = customtkinter.CTkLabel(master=self.frame_left,
                                              text="Worlde GUI",
                                              text_font=("Roboto Medium", -16))  # font name and size in px
        self.label_1.grid(row=1, column=0, pady=10, padx=10)

        self.button_1 = customtkinter.CTkButton(master=self.frame_left,
                                                text="New Game",
                                                command=self.button_event)
        self.button_1.grid(row=2, column=0, pady=10, padx=20)

        self.button_2 = customtkinter.CTkButton(master=self.frame_left,
                                                text="Next Guess",
                                                command=self.button_event)
        self.button_2.grid(row=3, column=0, pady=10, padx=20)

        self.label_mode = customtkinter.CTkLabel(master=self.frame_left, text="Appearance Mode:")
        self.label_mode.grid(row=9, column=0, pady=0, padx=20, sticky="w")

        self.optionmenu_1 = customtkinter.CTkOptionMenu(master=self.frame_left,
                                                        values=["Light", "Dark", "System"],
                                                        command=self.change_appearance_mode)
        self.optionmenu_1.grid(row=10, column=0, pady=10, padx=20, sticky="w")

        # ============ frame_right ============

        # configure grid layout (3x7)
        self.frame_right.rowconfigure((0, 1, 2, 3), weight=1)
        self.frame_right.rowconfigure(7, weight=10)
        self.frame_right.columnconfigure((0, 1), weight=1)
        self.frame_right.columnconfigure(2, weight=0)

        self.frame_info = customtkinter.CTkFrame(master=self.frame_right)
        self.frame_info.grid(row=0, column=0, columnspan=2, rowspan=4, pady=20, padx=20, sticky="nsew")

        # ============ frame_info ============

        # configure grid layout (1x1)
        self.frame_info.rowconfigure(0, weight=1)
        self.frame_info.columnconfigure(0, weight=1)

        # Make a grid of 5x6 labels 
        self.update_matrix_according_to_game_data()

        # Render a textfield to guess the word
        self.entry = customtkinter.CTkEntry(master=self.frame_right,
                                            width=120,
                                            placeholder_text="Enter your guess here")
        self.entry.grid(row=8, column=0, columnspan=2, pady=20, padx=20, sticky="we")

        self.optionmenu_1.set("Dark")


    def button_event(self):
        print("Button pressed")

    def change_appearance_mode(self, new_appearance_mode):
        customtkinter.set_appearance_mode(new_appearance_mode)

    def on_closing(self, event=0):
        self.destroy()



    def update_matrix_according_to_game_data(self):
        # Fill the grid with black labels -> from game_data.py
        for row in range(6):
            for column in range(5):
                self.label_info_1 = customtkinter.CTkLabel(master=self.frame_info,
                                                           text=" " ,
                                                           height=50,
                                                           width=50,
                                                           corner_radius=6,  # <- custom corner radius
                                                           fg_color=("white", "gray38"),  # <- custom tuple-color
                                                           justify=tkinter.LEFT)
                self.label_info_1.grid(column=column, row=row, padx=15, pady=15)

                # Fill the grid with black labels -> from game_data.py
                self.label_info_1.configure(text=f"{game_data.GAME_LETTERS[row][column]}")

                # Change the color of the labels -> from game_data.py
                if game_data.GAME_LETTERS[row][column] == "":
                    self.label_info_1.configure(fg_color=("grey", "gray38"))
                elif game_data.GAME_LETTERS[row][column] in game_data.WORD_TO_GUESS and game_data.WORD_TO_GUESS[column] != game_data.GAME_LETTERS[row][column]:
                    self.label_info_1.configure(fg_color=("yellow", "gray38"))
                elif game_data.WORD_TO_GUESS[column] == game_data.GAME_LETTERS[row][column]:
                    self.label_info_1.configure(fg_color=("green", "gray38"))
                else:
                    self.label_info_1.configure(fg_color=("white", "gray38"))


if __name__ == "__main__":
    app = App()
    app.mainloop()